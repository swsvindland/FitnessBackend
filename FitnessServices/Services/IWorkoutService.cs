using FitnessRepository.Models;

namespace FitnessServices.Services;

public interface IWorkoutService
{
    Task<IEnumerable<Workout>> GetWorkouts();
    Task<IEnumerable<WorkoutBlock>> GetWorkout(long workoutId);
}