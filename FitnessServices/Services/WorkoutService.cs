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

    public async Task<IEnumerable<WorkoutBlock>> GetWorkout(long workoutId)
    {
        return await _workoutRepository.GetWorkout(workoutId);
    }
}