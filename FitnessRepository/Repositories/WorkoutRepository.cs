using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public class WorkoutRepository: IWorkoutRepository
{
    private readonly FitnessContext _context;

    public WorkoutRepository(FitnessContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Workout>> GetWorkouts()
    {
        return await _context.Workout.ToListAsync();
    }

    public async Task<IEnumerable<WorkoutBlock>> GetWorkout(long workoutId)
    {
        var workoutBlocks = await _context.WorkoutBlock.Where(e => e.WorkoutId == workoutId).ToListAsync();

        foreach (var workoutBlock in workoutBlocks)
        {
            var exercises = await _context.WorkoutBlockExercise
                .Where(e => e.WorkoutBlockId == workoutBlock.Id)
                .Include(e => e.Exercise)
                .ToListAsync();
        
            workoutBlock.WorkoutBlockExercises = exercises;
        }

        return workoutBlocks;
    }
}