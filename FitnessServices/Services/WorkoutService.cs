using FitnessRepository.Models;
using FitnessRepository.Repositories;

namespace FitnessServices.Services;

public class WorkoutService: IWorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;
    
    public WorkoutService(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
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
}