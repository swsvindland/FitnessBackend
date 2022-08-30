using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IWorkoutRepository
{
    Task<IEnumerable<Workout>> GetWorkouts();
    Task<Workout?> GetWorkout(long workoutId);
    Task<IEnumerable<WorkoutBlock>> GetWorkoutBlocks(long workoutId);
    Task<IEnumerable<UserWorkout>> GetUserWorkouts(Guid userId);
    Task AddUserWorkout(UserWorkout workout);
    Task UpdateUserWorkout(UserWorkout workout);
    Task UpdateUserWorkouts(IEnumerable<UserWorkout> workouts);
    Task<IEnumerable<UserWorkoutActivity>> GetUserWorkoutActivities(Guid userId, long workoutBlockExerciseId);
    Task<UserWorkoutActivity?> GetUserWorkoutActivity(Guid userId, long workoutBlockExerciseId, int set);
    Task AddUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity);
    Task UpdateUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity);
}