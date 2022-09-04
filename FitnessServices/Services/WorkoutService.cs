using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;

namespace FitnessServices.Services;

public class WorkoutService: IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    
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
    
    public async Task<UserWorkoutActivityModel?> GetUserWorkoutActivity(Guid userId, long workoutBlockExerciseId, int set)
    {
        var workoutActivity = await _workoutRepository.GetUserWorkoutActivity(userId, workoutBlockExerciseId, set);
        var workout = await _workoutRepository.GetWorkoutBlock(workoutBlockExerciseId);

        if (workout == null)
        {
            return null;
        }
        
        if (workoutActivity == null)
        {
            var userExerciseOneRepMax = await GetUserOneRepMaxesByExerciseId(userId, workout.ExerciseId);

            var recommendedWeight = (int) Math.Floor(userExerciseOneRepMax?.Estimate * ( 1 + ( workout.MaxReps / 30)) * 0.8 ?? 0);
            var recommendedWeightByFive = (int) Math.Round(recommendedWeight / 5.0) * 5;
            
            return new UserWorkoutActivityModel()
            {
                UserId = userId,
                WorkoutBlockExerciseId = workoutBlockExerciseId,
                Set = set,
                Reps = workout.MaxReps,
                Weight = recommendedWeightByFive,
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
        var activity = new UserWorkoutActivity()
        {
            Id = userWorkoutActivity.Id,
            Created = DateTime.UtcNow.Date,
            UserId = userWorkoutActivity.UserId,
            WorkoutBlockExerciseId = userWorkoutActivity.WorkoutBlockExerciseId,
            Reps = userWorkoutActivity.Reps,
            Set = userWorkoutActivity.Set,
            Weight = userWorkoutActivity.Weight
        };
        
        if (userWorkoutActivity.Id == null)
        {
            await _workoutRepository.AddUserWorkoutActivity(activity);
        }
        else
        {
            await _workoutRepository.UpdateUserWorkoutActivity(activity);
        }
    }
    
    public async Task<IEnumerable<UserOneRepMaxEstimates>> GetUserOneRepMaxes(Guid userId)
    {
        return await _workoutRepository.GetUserOneRepMaxes(userId);
    }

    public async Task<UserOneRepMaxEstimates?> GetUserOneRepMaxesByExerciseId(Guid userId, long id)
    {
        return await _workoutRepository.GetUserOneRepMaxesByExerciseId(userId, id);
    }
}