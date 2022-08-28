using FitnessRepository.Models;

namespace FitnessServices.Services;

public interface IWorkoutService
{
    Task<IEnumerable<Workout>> GetWorkouts();
    Task<Workout?> GetWorkout(long workoutId);
    Task<IEnumerable<WorkoutBlock>> GetWorkoutBlocks(long workoutId);
}