using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IWorkoutRepository
{
    Task<IEnumerable<Exercise>> GetExercises();
    Task<IEnumerable<Workout>> GetWorkouts();
    Task<Workout?> GetWorkout(long workoutId);
    Task<IEnumerable<WorkoutBlock>> GetWorkoutBlocks(long workoutId);
    Task<WorkoutBlockExercise?> GetWorkoutBlock(long workoutBlockId);
    Task<IEnumerable<UserWorkout>> GetUserWorkouts(Guid userId);
    Task AddUserWorkout(UserWorkout workout);
    Task UpdateUserWorkout(UserWorkout workout);
    Task UpdateUserWorkouts(IEnumerable<UserWorkout> workouts);
    Task<IEnumerable<UserWorkoutActivity>> GetUserWorkoutActivities(Guid userId, long workoutBlockExerciseId);
    Task<UserWorkoutActivity?> GetUserWorkoutActivity(Guid userId, long workoutBlockExerciseId, int set, int week,
        int day);
    Task AddUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity);
    Task UpdateUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity);
    Task<IEnumerable<UserOneRepMaxEstimates>> GetUserOneRepMaxes(Guid userId);
    Task<UserOneRepMaxEstimates?> GetUserOneRepMaxesByExerciseId(Guid userId, long exerciseId);
    Task AddUserOneRepMax(UserOneRepMaxEstimates userOneRepMaxEstimates);
}