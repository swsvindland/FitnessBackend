using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;

namespace FitnessServices.Services;

public sealed class WorkoutService : IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;

    private readonly List<double> _repsToPercent = new()
    {
        1.05, 1.0, 0.94, .91, .88, .86, .83, .81, .79, .77, .75, .73, .71, .69, .67, .65, .63, .61, .59, .57, .55, .53, .51,
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
    
    public async Task<UserWorkoutExercise?> GetUserWorkoutExercise(Guid userId, long workoutExerciseId, int week, int day)
    {
        var workoutExercise = await _workoutRepository.GetWorkoutExercise(workoutExerciseId);
        var workoutActivities = await _workoutRepository.GetUserWorkoutActivities(userId, workoutExerciseId, week, day);
        
        if (workoutExercise == null)
        {
            return null;
        }
        
        var completedActivities = workoutActivities.ToDictionary(e => e.Set, e=> e);
        var activities = new List<UserWorkoutActivityModel>();

        for (var i = 0; i < workoutExercise.Sets; ++i)
        {
            if (completedActivities.ContainsKey(i))
            {
                activities.Add(new UserWorkoutActivityModel()
                {
                    Created = completedActivities[i].Created,
                    Id = completedActivities[i].Id,
                    Set = completedActivities[i].Set,
                    Reps = completedActivities[i].Reps,
                    Weight = completedActivities[i].Weight,
                    Time = completedActivities[i].Time,
                    Saved = true,
                    UserId = userId,
                    WorkoutExerciseId = workoutExerciseId
                });
            }
            else
            {
                var prevIndex = i - 1;
                var prevActivity = prevIndex >= 0 ? activities[i - 1] : null;
                
                var activity = await GetUserWorkoutActivityV2(userId, workoutExercise, prevActivity, i);

                if (activity != null)
                {
                    activities.Add(activity);
                }
            }
        }
        
        return new UserWorkoutExercise()
        {
            ExerciseId = workoutExercise.ExerciseId,
            Exercise = workoutExercise.Exercise,
            Sets = workoutExercise.Sets,
            MinReps = workoutExercise.MinReps,
            MaxReps = workoutExercise.MaxReps,
            Time = workoutExercise.Time,
            UserWorkoutActivities = activities
        };
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
    
    private async Task SetActiveCardioWorkout(Guid userId, long workoutId)
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
    
    private async Task<UserWorkoutActivityModel?> GetUserWorkoutActivityV2(Guid userId, WorkoutExercise workoutExercise, UserWorkoutActivityModel? prevWorkoutActivity,
        int set)
    {
        var userExerciseOneRepMax = await GetUserOneRepMaxesByExerciseId(userId, workoutExercise.ExerciseId);

        var recommendedWeight =
            (int) Math.Floor(userExerciseOneRepMax?.Estimate * _repsToPercent[(workoutExercise.MaxReps ?? 1) - 1] ?? 0);
        var recommendedWeightByFive = (int) Math.Round(recommendedWeight / 5.0) * 5;

        if (set == 0)
            return new UserWorkoutActivityModel()
            {
                UserId = userId,
                WorkoutExerciseId = workoutExercise.Id,
                Set = set,
                Reps = workoutExercise.MaxReps,
                Time = workoutExercise.Time,
                Weight = recommendedWeightByFive + 5,
                Created = DateTime.UtcNow,
                Saved = false
            };
        
        if (prevWorkoutActivity == null)
        {
            return new UserWorkoutActivityModel()
            {
                UserId = userId,
                WorkoutExerciseId = workoutExercise.Id,
                Set = set,
                Reps = workoutExercise.MaxReps,
                Time = workoutExercise.Time,
                Weight = recommendedWeightByFive,
                Created = DateTime.UtcNow,
                Saved = false
            };
        }

        return new UserWorkoutActivityModel()
        {
            UserId = userId,
            WorkoutExerciseId = workoutExercise.Id,
            Set = set,
            Reps = prevWorkoutActivity.Reps,
            Time = workoutExercise.Time,
            Weight = prevWorkoutActivity.Weight,
            Created = DateTime.UtcNow,
            Saved = false
        };
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
            (int) Math.Floor(userExerciseOneRepMax?.Estimate * _repsToPercent[(workout.MaxReps ?? 1) - 1] ?? 0);
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
            Day = userWorkoutActivity.Day,
            Time = userWorkoutActivity.Time
        };

        if (userWorkoutActivity.Id == null)
        {
            await _workoutRepository.AddUserWorkoutActivity(activity);
        }
        else
        {
            await _workoutRepository.UpdateUserWorkoutActivity(activity);
        }
        
        if (userWorkoutActivity.Time == null)
        {
            await _workoutRepository.AddUserOneRepMax(estimatedOneRepMaxModel);
        }
    }

    public async Task<IEnumerable<UserOneRepMaxEstimates>> GetUserOneRepMaxes(Guid userId)
    {
        return await _workoutRepository.GetUserOneRepMaxes(userId);
    }

    private async Task<UserOneRepMaxEstimates?> GetUserOneRepMaxesByExerciseId(Guid userId, long id)
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

        var workoutsCompletedInCurrentWorkout = workoutsCompleted.Where(x => x.WorkoutId == currentWorkout.WorkoutId);

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
                Day = 0,
                Week = 0,
                WorkoutId = currentWorkout.WorkoutId,
                WorkoutCompleted = true
            };
        }

        return new UserNextWorkout()
        {
            UserId = userId,
            Created = DateTime.UtcNow,
            Day = (short) nextDay,
            Week = (short) nextWeek,
            WorkoutId = currentWorkout.WorkoutId,
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
        if (workout?.UserId == null)
        {
            throw new Exception("Must have a user id");
        }
        
        await _workoutRepository.DeleteWorkout(workoutId);
    }
    
    public async Task<long> UpsertWorkoutExercise(UpdateWorkoutExercise workoutExercise)
    {
        if (workoutExercise.Id != null)
        {
            var updateWorkoutExercise = new WorkoutExercise()
            {
                Created = workoutExercise.Created,
                Updated = DateTime.UtcNow,
                Id = workoutExercise.Id.Value,
                ExerciseId = workoutExercise.ExerciseId,
                WorkoutId = workoutExercise.WorkoutId,
                Day = workoutExercise.Day,
                Sets = workoutExercise.Sets,
                MinReps = workoutExercise.MinReps,
                MaxReps = workoutExercise.MaxReps,
                Time = workoutExercise.Time,
                Order = workoutExercise.Order,
                RestTime = workoutExercise.RestTime,
            };
            return await _workoutRepository.UpdateWorkoutExercise(updateWorkoutExercise);
        }
        
        var newWorkoutExercise = new WorkoutExercise()
        {
            Created = DateTime.UtcNow,
            Updated = null,
            ExerciseId = workoutExercise.ExerciseId,
            WorkoutId = workoutExercise.WorkoutId,
            Day = workoutExercise.Day,
            Sets = workoutExercise.Sets,
            MinReps = workoutExercise.MinReps,
            MaxReps = workoutExercise.MaxReps,
            Time = workoutExercise.Time,
            Order = workoutExercise.Order,
            RestTime = workoutExercise.RestTime,
        };
        
        return await _workoutRepository.AddWorkoutExercise(newWorkoutExercise);
    }
}