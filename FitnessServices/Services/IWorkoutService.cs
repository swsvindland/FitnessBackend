using FitnessRepository.Models;
using FitnessServices.Models;

namespace FitnessServices.Services;

public interface IWorkoutService
{
    Task<IEnumerable<Exercise>> GetExercises();
    Task<IEnumerable<Workout>> GetWorkouts();
    Task<IEnumerable<Workout>> GetCardioWorkouts();
    Task<IEnumerable<Workout>> GetWorkoutsByUserId(Guid userId);
    Task<Workout?> GetWorkout(long workoutId);
    Task<IEnumerable<WorkoutExercise>> GetWorkoutExercises(long workoutId, int day);
    Task<IEnumerable<WorkoutExercise>> GetWorkoutExercises(long workoutId);
    Task<IEnumerable<UserWorkout>> GetUserWorkouts(Guid userId);
    Task BuyWorkout(Guid userId, long workoutId);
    Task SetActiveWorkout(Guid userId, long workoutId);
    Task<UserWorkoutExercise> GetUserWorkoutExercise(Guid userId, long workoutExerciseId, int week, int day);
    Task<IEnumerable<UserWorkoutActivity>> GetUserWorkoutActivities(Guid userId, long workoutExerciseId);
    Task<UserWorkoutActivityModel?> GetUserWorkoutActivity(Guid userId, long workoutExerciseId, int set, int week,
        int day);
    Task AddUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity);
    Task<IEnumerable<UserOneRepMaxEstimates>> GetUserOneRepMaxes(Guid userId);
    Task UserCompleteWorkout(UserWorkoutsCompleted userWorkoutsCompleted);
    Task<IEnumerable<UserWorkoutsCompleted>> GetUserWorkoutsCompleted(Guid userId);
    Task<UserNextWorkout?> GetUserNextWorkout(Guid userId);
    Task<UserNextWorkout?> GetUserNextCardioWorkout(Guid userId);
    Task RestartWorkout(Guid userId, long workoutId);
    Task<long> AddWorkout(Workout workout);
    Task<long> EditWorkout(Workout workout);
    Task DeleteWorkout(long workoutId);

    Task<long> UpsertWorkoutExercise(WorkoutExercise workoutExercise);
}