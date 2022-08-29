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
}