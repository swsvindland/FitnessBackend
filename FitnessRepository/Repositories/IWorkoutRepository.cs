using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IWorkoutRepository
{
    Task<IEnumerable<Exercise>> GetExercises();
    Task<IEnumerable<Workout>> GetWorkouts();
    Task<Workout?> GetWorkout(long workoutId);
    Task<IEnumerable<Workout>> GetWorkoutsByUserId(Guid userId);
    Task<IEnumerable<WorkoutExercise>> GetWorkoutExercises(long workoutId);
    Task<IEnumerable<WorkoutExercise>> GetWorkoutExercises(long workoutId, int day);
    Task<WorkoutExercise?> GetWorkoutExercise(long workoutExerciseId);
    Task<IEnumerable<UserWorkout>> GetUserWorkouts(Guid userId);
    Task<UserWorkout?> GetActiveUserWorkouts(Guid userId);
    Task AddUserWorkout(UserWorkout workout);
    Task UpdateUserWorkout(UserWorkout workout);
    Task UpdateUserWorkouts(IEnumerable<UserWorkout> workouts);
    Task<IEnumerable<UserWorkoutActivity>> GetUserWorkoutActivities(Guid userId, long workoutExerciseId);

    Task<IEnumerable<UserWorkoutActivity>> GetUserWorkoutActivities(Guid userId,
        long workoutExerciseId, int week, int day);
    Task DeleteUserWorkoutActivities(IEnumerable<UserWorkoutActivity> activities);
    Task<UserWorkoutActivity?> GetUserWorkoutActivity(Guid userId, long workoutExerciseId, int set, int week,
        int day);
    Task AddUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity);
    Task UpdateUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity);
    Task<IEnumerable<UserOneRepMaxEstimates>> GetUserOneRepMaxes(Guid userId);
    Task<UserOneRepMaxEstimates?> GetUserOneRepMaxesByExerciseId(Guid userId, long exerciseId);
    Task AddUserOneRepMax(UserOneRepMaxEstimates userOneRepMaxEstimates);
    Task AddUserWorkoutCompleted(UserWorkoutsCompleted userWorkoutsCompleted);
    Task<IEnumerable<UserWorkoutsCompleted>> GetUserWorkoutsCompleted(Guid userId);
    Task DeleteUserWorkoutCompleted(IEnumerable<UserWorkoutsCompleted> workoutsCompleted);

    Task<long> AddWorkout(Workout workout);

    Task<long> UpdateWorkout(Workout workout);

    Task DeleteWorkout(long workoutId);

    Task<long> AddWorkoutExercise(WorkoutExercise workoutExercise);

    Task<long> UpdateWorkoutExercise(WorkoutExercise workoutExercise);
    Task<UserWorkoutSubstitution?> GetUserWorkoutSubstitution(Guid userId, long workoutExerciseId);
    Task<long> AddUserWorkoutSubstitution(UserWorkoutSubstitution userWorkoutSubstitution);
    Task UpdateUserWorkoutSubstitution(UserWorkoutSubstitution userWorkoutSubstitution);
    Task DeleteUserWorkoutSubstitution(long id);
}