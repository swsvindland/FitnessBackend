using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;

namespace FitnessServices.Services;

public sealed class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;

    private List<double> REPS_TO_PERCENT = new List<double>
    {
        1.0, 0.94, .91, .88, .86, .83, .81, .79, .77, .75, .73, .71, .69, .67, .65, .63, .61, .59, .57, .55, .53, .51,
        .49, .47, .45, .43, .41, .39, .37, .35, .33, .31, .29, .27, .25, .23, .21, .19, .17, .15, .13, .11, .09, .07,
        .05, .03, .01
    };

    public WorkoutService(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    public async Task<IEnumerable<Exercise>> GetExercises()
    {
        return await _workoutRepository.GetExercises();
    }

    public async Task<IEnumerable<Workout>> GetWorkouts()
    {
        return await _workoutRepository.GetWorkouts();
    }
    
    public async Task<IEnumerable<Workout>> GetCardioWorkouts()
    {
        return await _workoutRepository.GetCardioWorkouts();
    }
    
    public async Task<IEnumerable<Workout>> GetWorkoutsByUserId(Guid userId)
    {
        return await _workoutRepository.GetWorkoutsByUserId(userId);
    }    

    public async Task<Workout?> GetWorkout(long workoutId)
    {
        return await _workoutRepository.GetWorkout(workoutId);
    }

    public async Task<IEnumerable<WorkoutExercise>> GetWorkoutExercises(long workoutId, int day)
    {
        return await _workoutRepository.GetWorkoutExercises(workoutId, day);
    }
    
    public async Task<IEnumerable<WorkoutExercise>> GetWorkoutExercises(long workoutId)
    {
        return await _workoutRepository.GetWorkoutExercises(workoutId);
    }
    
    public async Task<IEnumerable<UserWorkout>> GetUserWorkouts(Guid userId)
    {
        return await _workoutRepository.GetUserWorkouts(userId);
    }

    public async Task BuyWorkout(Guid userId, long workoutId)
    {
        var workout = await _workoutRepository.GetWorkout(workoutId);

        // should set all to inactive as workoutId is not added
        if (workout?.Type == WorkoutType.Cardio)
        {
            await SetActiveCardioWorkout(userId, workoutId);
        }
        else
        {
            await SetActiveWorkout(userId, workoutId);
        }
        
        await _workoutRepository.AddUserWorkout(new UserWorkout()
        {
            UserId = userId,
            Active = true,
            WorkoutId = workoutId,
            Created = DateTime.UtcNow
        });
    }

    public async Task SetActiveWorkout(Guid userId, long workoutId)
    {
        var userWorkouts = (await _workoutRepository.GetUserWorkouts(userId))
            .Where(e => e.Workout?.Type is WorkoutType.Unknown or WorkoutType.Resistance);
        var enumerable = userWorkouts as UserWorkout[] ?? userWorkouts.ToArray();

        foreach (var userWorkout in enumerable)
        {
            userWorkout.Active = userWorkout.WorkoutId == workoutId;
        }

        await _workoutRepository.UpdateUserWorkouts(enumerable);
    }
    
    public async Task SetActiveCardioWorkout(Guid userId, long workoutId)
    {
        var userWorkouts = (await _workoutRepository.GetUserWorkouts(userId))
            .Where(e => e.Workout?.Type is WorkoutType.Cardio);
        var enumerable = userWorkouts as UserWorkout[] ?? userWorkouts.ToArray();

        foreach (var userWorkout in enumerable)
        {
            userWorkout.Active = userWorkout.WorkoutId == workoutId;
        }

        await _workoutRepository.UpdateUserWorkouts(enumerable);
    }

    public async Task<IEnumerable<UserWorkoutActivity>> GetUserWorkoutActivities(Guid userId,
        long workoutExerciseId)
    {
        return await _workoutRepository.GetUserWorkoutActivities(userId, workoutExerciseId);
    }

    public async Task<UserWorkoutActivityModel?> GetUserWorkoutActivity(Guid userId, long workoutExerciseId,
        int set, int week, int day)
    {
        var workoutActivity =
            await _workoutRepository.GetUserWorkoutActivity(userId, workoutExerciseId, set, week, day);
        var workout = await _workoutRepository.GetWorkoutExercise(workoutExerciseId);

        if (workout == null)
        {
            return null;
        }

        if (workoutActivity != null)
        {
            var newWorkoutActivity = new UserWorkoutActivityModel()
            {
                Id = workoutActivity.Id,
                UserId = workoutActivity.UserId,
                WorkoutExerciseId = workoutActivity.WorkoutExerciseId,
                Set = workoutActivity.Set,
                Reps = workoutActivity.Reps,
                Time = workout.Time,
                Weight = workoutActivity.Weight,
                Created = DateTime.UtcNow,
                Saved = true
            };

            return newWorkoutActivity;
        }

        var userExerciseOneRepMax = await GetUserOneRepMaxesByExerciseId(userId, workout.ExerciseId);

        var recommendedWeight =
            (int) Math.Floor(userExerciseOneRepMax?.Estimate * REPS_TO_PERCENT[(workout.MaxReps ?? 1) - 1] ?? 0);
        var recommendedWeightByFive = (int) Math.Round(recommendedWeight / 5.0) * 5;

        if (set == 0)
            return new UserWorkoutActivityModel()
            {
                UserId = userId,
                WorkoutExerciseId = workoutExerciseId,
                Set = set,
                Reps = workout.MaxReps,
                Time = workout.Time,
                Weight = recommendedWeightByFive + 5,
                Created = DateTime.UtcNow,
                Saved = false
            };

        var prevWorkoutActivity =
            await _workoutRepository.GetUserWorkoutActivity(userId, workoutExerciseId, set - 1, week, day);

        if (prevWorkoutActivity == null)
        {
            return new UserWorkoutActivityModel()
            {
                UserId = userId,
                WorkoutExerciseId = workoutExerciseId,
                Set = set,
                Reps = workout.MaxReps,
                Time = workout.Time,
                Weight = recommendedWeightByFive,
                Created = DateTime.UtcNow,
                Saved = false
            };
        }

        return new UserWorkoutActivityModel()
        {
            UserId = userId,
            WorkoutExerciseId = workoutExerciseId,
            Set = set,
            Reps = prevWorkoutActivity.Reps,
            Time = workout.Time,
            Weight = prevWorkoutActivity.Weight,
            Created = DateTime.UtcNow,
            Saved = false
        };
    }

    public async Task AddUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity)
    {
        var workoutExercise =
            await _workoutRepository.GetWorkoutExercise(userWorkoutActivity.WorkoutExerciseId);
        var estimatedOneRepMax =
            (int) Math.Floor((userWorkoutActivity.Weight ?? 0) * (1.0 + ((userWorkoutActivity.Reps ?? 0) / 30.0)));

        if (workoutExercise == null)
        {
            return;
        }

        var estimatedOneRepMaxModel = new UserOneRepMaxEstimates()
        {
            UserId = userWorkoutActivity.UserId,
            ExerciseId = workoutExercise.ExerciseId,
            Estimate = estimatedOneRepMax,
            Created = DateTime.UtcNow,
        };

        var activity = new UserWorkoutActivity()
        {
            Id = userWorkoutActivity.Id,
            WorkoutExerciseId = userWorkoutActivity.WorkoutExerciseId,
            Created = DateTime.UtcNow.Date,
            UserId = userWorkoutActivity.UserId,
            Reps = userWorkoutActivity.Reps,
            Set = userWorkoutActivity.Set,
            Weight = userWorkoutActivity.Weight,
            Week = userWorkoutActivity.Week,
            Day = userWorkoutActivity.Day
        };

        if (userWorkoutActivity.Id == null)
        {
            await _workoutRepository.AddUserWorkoutActivity(activity);
        }
        else
        {
            await _workoutRepository.UpdateUserWorkoutActivity(activity);
        }

        await _workoutRepository.AddUserOneRepMax(estimatedOneRepMaxModel);
    }

    public async Task<IEnumerable<UserOneRepMaxEstimates>> GetUserOneRepMaxes(Guid userId)
    {
        return await _workoutRepository.GetUserOneRepMaxes(userId);
    }

    public async Task<UserOneRepMaxEstimates?> GetUserOneRepMaxesByExerciseId(Guid userId, long id)
    {
        return await _workoutRepository.GetUserOneRepMaxesByExerciseId(userId, id);
    }

    public async Task UserCompleteWorkout(UserWorkoutsCompleted userWorkoutsCompleted)
    {
        var userWorkout = new UserWorkoutsCompleted
        {
            UserId = userWorkoutsCompleted.UserId,
            Created = DateTime.UtcNow,
            Day = userWorkoutsCompleted.Day,
            Week = userWorkoutsCompleted.Week,
            WorkoutId = userWorkoutsCompleted.WorkoutId,
        };

        await _workoutRepository.AddUserWorkoutCompleted(userWorkout);
    }
    
    public async Task<IEnumerable<UserWorkoutsCompleted>> GetUserWorkoutsCompleted(Guid userId)
    {
        return await _workoutRepository.GetUserWorkoutsCompleted(userId);
    }

    public async Task<UserNextWorkout?> GetUserNextWorkout(Guid userId)
    {
        var currentWorkout = await _workoutRepository.GetActiveUserWorkouts(userId);
        var workoutsCompleted = (await _workoutRepository.GetUserWorkoutsCompleted(userId)).ToArray();
        
        if (currentWorkout == null)
        {
            return null;
        }

        return await GetNextWorkout(userId, currentWorkout, workoutsCompleted);
    }
    
    public async Task<UserNextWorkout?> GetUserNextCardioWorkout(Guid userId)
    {
        var currentWorkout = await _workoutRepository.GetActiveUserCardioWorkouts(userId);
        var workoutsCompleted = await _workoutRepository.GetUserWorkoutsCompleted(userId);
        
        if (currentWorkout == null)
        {
            return null;
        }

        return await GetNextWorkout(userId, currentWorkout, workoutsCompleted);
    }

    private async Task<UserNextWorkout?> GetNextWorkout(Guid userId, UserWorkout currentWorkout, IEnumerable<UserWorkoutsCompleted> workoutsCompleted)
    {
        var workout = await _workoutRepository.GetWorkout(currentWorkout.WorkoutId);

        var workoutsCompletedInCurrentWorkout = workoutsCompleted.Where(x => x.WorkoutId == currentWorkout?.WorkoutId);

        var lastCompletedWorkout = workoutsCompletedInCurrentWorkout.MaxBy(x => x.Created);

        var days = workout?.Days ?? 1;
        var weeks = workout?.Duration ?? 1;

        var nextDay = lastCompletedWorkout?.Day + 1 > days ? 1 : lastCompletedWorkout?.Day + 1 ?? 1;
        var nextWeek = lastCompletedWorkout?.Day >= days
            ? lastCompletedWorkout.Week + 1
            : lastCompletedWorkout?.Week ?? 1;

        // workout would be complete
        if (nextWeek > weeks)
        {
            return new UserNextWorkout()
            {
                UserId = userId,
                Created = DateTime.UtcNow,
                Day = (short) 0,
                Week = (short) 0,
                WorkoutId = currentWorkout?.WorkoutId ?? 0,
                WorkoutCompleted = true
            };
        }

        return new UserNextWorkout()
        {
            UserId = userId,
            Created = DateTime.UtcNow,
            Day = (short) nextDay,
            Week = (short) nextWeek,
            WorkoutId = currentWorkout?.WorkoutId ?? 0,
            WorkoutCompleted = false
        };
    }
    public async Task RestartWorkout(Guid userId, long workoutId)
    {
        var workoutExercises = await _workoutRepository.GetWorkoutExercises(workoutId);

        foreach (var workoutExercise in workoutExercises)
        {
            var workoutActivities =  await _workoutRepository.GetUserWorkoutActivities(userId, workoutExercise.Id);

            await _workoutRepository.DeleteUserWorkoutActivities(workoutActivities);
        }
        
        var userWorkoutsCompleted = await _workoutRepository.GetUserWorkoutsCompleted(userId);
        
        await _workoutRepository.DeleteUserWorkoutCompleted(userWorkoutsCompleted);
    }
    
    public async Task<long> AddWorkout(Workout workout)
    {
        if (workout.UserId == null)
        {
            throw new Exception("Must have a user id");
        }
        
        workout.Created = DateTime.UtcNow;
        workout.Updated = DateTime.UtcNow;
        return await _workoutRepository.AddWorkout(workout);
    }
    
    public async Task<long> EditWorkout(Workout workout)
    {
        workout.Updated = DateTime.UtcNow;
        return await _workoutRepository.UpdateWorkout(workout);
    }
    
    public async Task DeleteWorkout(long workoutId)
    {
        var workout = await GetWorkout(workoutId);
        if (workout.UserId == null)
        {
            throw new Exception("Must have a user id");
        }
        
        await _workoutRepository.DeleteWorkout(workoutId);
    }
    
    public async Task<long> UpsertWorkoutExercise(WorkoutExercise workoutExercise)
    {
        workoutExercise.Updated = DateTime.UtcNow;
    
        if (workoutExercise.Id == null)
        {
            workoutExercise.Created = DateTime.UtcNow;
            return await _workoutRepository.AddWorkoutExercise(workoutExercise);
        }
        return await _workoutRepository.UpdateWorkoutExercise(workoutExercise);
    }
}