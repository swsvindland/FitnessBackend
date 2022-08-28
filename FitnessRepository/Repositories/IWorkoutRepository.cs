using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IWorkoutRepository
{
    Task<IEnumerable<Workout>> GetWorkouts();
    Task<IEnumerable<WorkoutBlock>> GetWorkout(long workoutId);
}