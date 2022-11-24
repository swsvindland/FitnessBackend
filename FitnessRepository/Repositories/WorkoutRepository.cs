using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public sealed class WorkoutRepository : IWorkoutRepository
{
    private readonly FitnessContext _context;

    public WorkoutRepository(FitnessContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Exercise>> GetExercises()
    {
        return await _context.Exercise.ToListAsync();
    }

    public async Task<IEnumerable<Workout>> GetWorkouts()
    {
        return await _context.Workout.ToListAsync();
    }

    public async Task<Workout?> GetWorkout(long workoutId)
    {
        return await _context.Workout.FirstOrDefaultAsync(e => e.Id == workoutId);
    }

    public async Task<IEnumerable<WorkoutExercise>> GetWorkoutExercises(long workoutId)
    {
        var workoutExercise = await _context.WorkoutExercise
            .Where(e => e.WorkoutId == workoutId)
            .ToListAsync();

        return workoutExercise;
    }

    public async Task<IEnumerable<WorkoutExercise>> GetWorkoutExercises(long workoutId, int day)
    {
        var workoutExercise = await _context.WorkoutExercise
            .Where(e => e.WorkoutId == workoutId)
            .Where(e => e.Day == day)
            .Include(e => e.Exercise)
            .ToListAsync();

        return workoutExercise;
    }

    
    public async Task<WorkoutExercise?> GetWorkoutExercise(long workoutExerciseId)
    {
        var workoutExercise = await _context.WorkoutExercise
            .Where(e => e.Id == workoutExerciseId)
            .Include(e => e.Exercise)
            .FirstOrDefaultAsync();

        return workoutExercise;
    }

    public async Task<IEnumerable<UserWorkout>> GetUserWorkouts(Guid userId)
    {
        return await _context.UserWorkout.Where(e => e.UserId == userId).ToListAsync();
    }

    public async Task<UserWorkout?> GetActiveUserWorkouts(Guid userId)
    {
        return await _context.UserWorkout.Where(e => e.UserId == userId).Where(e => e.Active).FirstOrDefaultAsync();
    }

    public async Task AddUserWorkout(UserWorkout workout)
    {
        _context.UserWorkout.Add(workout);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserWorkout(UserWorkout workout)
    {
        _context.UserWorkout.Update(workout);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserWorkouts(IEnumerable<UserWorkout> workouts)
    {
        _context.UserWorkout.UpdateRange(workouts);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserWorkoutActivity>> GetUserWorkoutActivities(Guid userId,
        long workoutExerciseId)
    {
        return await _context.UserWorkoutActivity
            .Where(e => e.UserId == userId)
            .Where(e => e.WorkoutExerciseId == workoutExerciseId)
            .Where(e => e.Created == DateTime.UtcNow.Date)
            .ToListAsync();
    }

    public async Task DeleteUserWorkoutActivities(IEnumerable<UserWorkoutActivity> activities)
    {
        _context.UserWorkoutActivity.RemoveRange(activities);

        await _context.SaveChangesAsync();
    }

    public async Task<UserWorkoutActivity?> GetUserWorkoutActivity(Guid userId, long workoutExerciseId, int set,
        int week, int day)
    {
        return await _context.UserWorkoutActivity
            .Where(e => e.UserId == userId)
            .Where(e => e.WorkoutExerciseId == workoutExerciseId)
            .Where(e => e.Set == set)
            .Where(e => e.Week == week)
            .Where(e => e.Day == day)
            .FirstOrDefaultAsync();
    }

    public async Task AddUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity)
    {
        _context.UserWorkoutActivity.Add(userWorkoutActivity);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserWorkoutActivity(UserWorkoutActivity userWorkoutActivity)
    {
        _context.UserWorkoutActivity.Update(userWorkoutActivity);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserOneRepMaxEstimates>> GetUserOneRepMaxes(Guid userId)
    {
        return await _context.UserOneRepMaxEstimates.Where(e => e.UserId == userId).ToListAsync();
    }

    public async Task<UserOneRepMaxEstimates?> GetUserOneRepMaxesByExerciseId(Guid userId, long exerciseId)
    {
        return await _context.UserOneRepMaxEstimates
            .Where(e => e.UserId == userId)
            .Where(e => e.ExerciseId == exerciseId)
            .OrderByDescending(e => e.Created)
            .FirstOrDefaultAsync();
    }

    public async Task AddUserOneRepMax(UserOneRepMaxEstimates userOneRepMaxEstimates)
    {
        _context.UserOneRepMaxEstimates.Add(userOneRepMaxEstimates);

        await _context.SaveChangesAsync();
    }

    public async Task AddUserWorkoutCompleted(UserWorkoutsCompleted userWorkoutsCompleted)
    {
        _context.UserWorkoutsCompleted.Add(userWorkoutsCompleted);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserWorkoutsCompleted>> GetUserWorkoutsCompleted(Guid userId)
    {
        return await _context.UserWorkoutsCompleted.Where(e => e.UserId == userId).OrderBy(e => e.Created).ToListAsync();
    }

    public async Task DeleteUserWorkoutCompleted(IEnumerable<UserWorkoutsCompleted> workoutsCompleted)
    {
        _context.UserWorkoutsCompleted.RemoveRange(workoutsCompleted);

        await _context.SaveChangesAsync();
    }
}