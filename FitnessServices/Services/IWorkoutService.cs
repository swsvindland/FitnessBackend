using FitnessRepository.Models;

namespace FitnessServices.Services;

public interface IWorkoutService
{
    Task<IEnumerable<Workout>> GetWorkouts();
    Task<Workout?> GetWorkout(long workoutId);
    Task<IEnumerable<WorkoutBlock>> GetWorkoutBlocks(long workoutId);
    Task<IEnumerable<UserWorkout>> GetUserWorkouts(Guid userId);
    Task BuyWorkout(Guid userId, long workoutId);
    Task SetActiveWorkout(Guid userId, long workoutId);
    Task<IEnumerable<UserWorkoutActivity>> GetUserWorkoutActivities(Guid userId, long workoutBlockExerciseId);
    Task<UserWorkoutActivity?> GetUserWorkoutActivity(Guid userId, long workoutBlockExerciseId, int set);
    Task AddUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity);
}