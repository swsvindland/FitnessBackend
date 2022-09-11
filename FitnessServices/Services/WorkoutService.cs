using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;

namespace FitnessServices.Services;

public class WorkoutService: IWorkoutService
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
    
    public async Task<Workout?> GetWorkout(long workoutId)
    {
        return await _workoutRepository.GetWorkout(workoutId);
    }

    public async Task<IEnumerable<WorkoutBlock>> GetWorkoutBlocks(long workoutId)
    {
        return await _workoutRepository.GetWorkoutBlocks(workoutId);
    }
    
    public async Task<IEnumerable<UserWorkout>> GetUserWorkouts(Guid userId)
    {
        return await _workoutRepository.GetUserWorkouts(userId);
    }
    
    public async Task BuyWorkout(Guid userId, long workoutId)
    {
        // should set all to inactive as workoutId is not added
        await SetActiveWorkout(userId, workoutId);
        
        // TODO: Payment processing for paid workouts

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
        var userWorkouts = await _workoutRepository.GetUserWorkouts(userId);
        var enumerable = userWorkouts as UserWorkout[] ?? userWorkouts.ToArray();
        
        foreach (var userWorkout in enumerable)
        {
            userWorkout.Active = userWorkout.WorkoutId == workoutId;
        }

        await _workoutRepository.UpdateUserWorkouts(enumerable);
    }

    public async Task<IEnumerable<UserWorkoutActivity>> GetUserWorkoutActivities(Guid userId, long workoutBlockExerciseId)
    {
        return await _workoutRepository.GetUserWorkoutActivities(userId, workoutBlockExerciseId);
    }
    
    public async Task<UserWorkoutActivityModel?> GetUserWorkoutActivity(Guid userId, long workoutBlockExerciseId, int set, int week, int day)
    {
        var workoutActivity = await _workoutRepository.GetUserWorkoutActivity(userId, workoutBlockExerciseId, set, week, day);
        var workout = await _workoutRepository.GetWorkoutBlockExercise(workoutBlockExerciseId);

        if (workout == null)
        {
            return null;
        }
        
        if (workoutActivity == null)
        {
            var userExerciseOneRepMax = await GetUserOneRepMaxesByExerciseId(userId, workout.ExerciseId);

            var recommendedWeight = (int) Math.Floor(userExerciseOneRepMax?.Estimate * REPS_TO_PERCENT[workout.MaxReps + 1] ?? 0);
            var recommendedWeightByFive = (int) Math.Round(recommendedWeight / 5.0) * 5;
            
            return new UserWorkoutActivityModel()
            {
                UserId = userId,
                WorkoutBlockExerciseId = workoutBlockExerciseId,
                Set = set,
                Reps = workout.MaxReps,
                Weight = recommendedWeightByFive + 10, // increase the estimate by 10 for progressive overload
                Created = DateTime.UtcNow,
                Saved = false
            };
        }
        
        var newWorkoutActivity = new UserWorkoutActivityModel()
        {
            UserId = workoutActivity.UserId,
            WorkoutBlockExerciseId = workoutActivity.WorkoutBlockExerciseId,
            Set = workoutActivity.Set,
            Reps = workoutActivity.Reps,
            Weight = workoutActivity.Weight,
            Created = DateTime.UtcNow,
            Saved = true
        };
        
        return newWorkoutActivity;
    }

    public async Task AddUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity)
    {
        var exerciseBlockActivity = await _workoutRepository.GetWorkoutBlockExercise(userWorkoutActivity.WorkoutBlockExerciseId);
        var estimatedOneRepMax = (int)Math.Floor(userWorkoutActivity.Weight * (1.0 + (userWorkoutActivity.Reps / 30.0)));

        if (exerciseBlockActivity == null)
        {
            return;
        }
        
        var estimatedOneRepMaxModel = new UserOneRepMaxEstimates()
        {
            UserId = userWorkoutActivity.UserId,
            ExerciseId = exerciseBlockActivity.ExerciseId,
            Estimate = estimatedOneRepMax,
            Created = DateTime.UtcNow,
        };
        
        var activity = new UserWorkoutActivity()
        {
            Id = userWorkoutActivity.Id,
            Created = DateTime.UtcNow.Date,
            UserId = userWorkoutActivity.UserId,
            WorkoutBlockExerciseId = userWorkoutActivity.WorkoutBlockExerciseId,
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
            WorkoutBlock = userWorkoutsCompleted.WorkoutBlock
        };
        
        await _workoutRepository.AddUserWorkoutCompleted(userWorkout);
    }

    public async Task<UserWorkoutsCompleted?> GetUserNextWorkout(Guid userId)
    {
        var currentWorkout = await _workoutRepository.GetActiveUserWorkouts(userId);
        var workoutsCompleted = (await _workoutRepository.GetUserWorkoutsCompleted(userId)).ToArray();
        var workoutDetails = await _workoutRepository.GetWorkoutBlock(workoutsCompleted.FirstOrDefault()?.WorkoutId ?? 0);
        
        var workoutsCompletedInCurrentWorkout = workoutsCompleted.Where(x => x.WorkoutId == currentWorkout?.WorkoutId);
        
        var lastCompletedWorkout = workoutsCompletedInCurrentWorkout.MaxBy(x => x.Created);

        var days = workoutDetails?.Days ?? 1;
        var weeks = workoutDetails?.Duration ?? 1;
        
        var nextDay = lastCompletedWorkout?.Day + 1 > days ? 1 : lastCompletedWorkout?.Day + 1 ?? 1;
        var nextWeek = lastCompletedWorkout?.Day > days ? lastCompletedWorkout.Week + 1 : lastCompletedWorkout?.Week ?? 1;

        // workout would be complete
        if (nextWeek > weeks)
        {
            return null;
        }
        
        return new UserWorkoutsCompleted()
        {
            UserId = userId,
            Created = DateTime.UtcNow,
            Day = (short) nextDay,
            Week = (short) nextWeek,
            WorkoutId = currentWorkout?.WorkoutId ?? 0,
            WorkoutBlock = 1
        };
    }
}