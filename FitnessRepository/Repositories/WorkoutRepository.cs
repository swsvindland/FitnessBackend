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
        return await _context.Workout.Where(e => e.UserId == null).ToListAsync();
    }
    
    public async Task<IEnumerable<Workout>> GetWorkoutsByUserId(Guid userId)
    {
        return await _context.Workout.Where(e => e.UserId == userId).ToListAsync();
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
    
    public async Task<long> AddWorkout(Workout workout)
    {
        _context.Workout.Add(workout);
    
        await _context.SaveChangesAsync();
        return workout.Id;
    }
    
    public async Task<long> UpdateWorkout(Workout workout)
    {
        _context.Workout.Update(workout);
    
        await _context.SaveChangesAsync();
        return workout.Id;
    }
    
    public async Task DeleteWorkout(long workoutId)
    {
        var workout = await _context.Workout.FirstOrDefaultAsync(e => e.Id == workoutId);

        if (workout == null) return;
        
        _context.UserWorkout.RemoveRange(_context.UserWorkout.Where(e => e.WorkoutId == workoutId));
        _context.UserWorkoutsCompleted.RemoveRange(_context.UserWorkoutsCompleted.Where(e => e.WorkoutId == workoutId));

        var workoutExercises = await _context.WorkoutExercise.Where(e => e.WorkoutId == workoutId).ToListAsync();

        foreach (var workoutExercise in workoutExercises)
        {
            var userWorkoutActivities = await _context.UserWorkoutActivity
                .Where(e => e.WorkoutExerciseId == workoutExercise.Id)
                .ToListAsync();

            _context.UserWorkoutActivity.RemoveRange(userWorkoutActivities);
        }
        
        _context.WorkoutExercise.RemoveRange(workoutExercises);
        _context.Workout.Remove(workout);
        
        await _context.SaveChangesAsync();
    }
    
    public async Task<long> AddWorkoutExercise(WorkoutExercise workoutExercise)
    {
        _context.WorkoutExercise.Add(workoutExercise);
    
        await _context.SaveChangesAsync();
        return workoutExercise.Id;
    }

    public async Task<long> UpdateWorkoutExercise(WorkoutExercise workoutExercise)
    {
        _context.WorkoutExercise.Update(workoutExercise);
    
        await _context.SaveChangesAsync();
        return workoutExercise.Id;
    }
    
    // public async Task<UserCustomWorkout?> GetUserCustomWorkout(Guid userId, long id)
    // {
    //     return await _context.UserCustomWorkout
    //         .Where(e => e.UserId == userId)
    //         .Where(e => e.Id == id)
    //         .FirstOrDefaultAsync();
    // }
    //
    // public async Task<IEnumerable<UserCustomWorkout>> GetUserCustomWorkouts(Guid userId)
    // {
    //     return await _context.UserCustomWorkout
    //         .Where(e => e.UserId == userId)
    //         .ToListAsync();
    // }
    //
    // public async Task<long> AddUserCustomWorkout(UserCustomWorkout userCustomWorkout)
    // {
    //     _context.UserCustomWorkout.Add(userCustomWorkout);
    //
    //     await _context.SaveChangesAsync();
    //     return userCustomWorkout.Id;
    // }
    //
    // public async Task<long> UpdateUserCustomWorkout(UserCustomWorkout userCustomWorkout)
    // {
    //     _context.UserCustomWorkout.Update(userCustomWorkout);
    //
    //     await _context.SaveChangesAsync();
    //     return userCustomWorkout.Id;
    // }
    //
    // public async Task DeleteUserCustomWorkout(Guid userId, long userCustomWorkoutId)
    // {
    //     var customWorkout = await GetUserCustomWorkout(userId, userCustomWorkoutId);
    //     var customWorkoutExercises = await GetUserCustomWorkoutExercises(userCustomWorkoutId);
    //     var customWorkoutActivities = await GetUserCustomWorkoutActivities(userId, userCustomWorkoutId);
    //     var workouts = (await GetUserWorkouts(userId)).Where(e => e.UserCustomWorkoutId == userCustomWorkoutId).ToList();
    //     var workoutsCompleted =
    //         (await GetUserWorkoutsCompleted(userId)).Where(e => e.UserCustomWorkoutId == userCustomWorkoutId);
    //
    //     _context.UserWorkoutsCompleted.RemoveRange(workoutsCompleted);
    //     _context.UserWorkout.RemoveRange(workouts);
    //     _context.UserCustomWorkoutActivity.RemoveRange(customWorkoutActivities);
    //     _context.UserCustomWorkoutExercise.RemoveRange(customWorkoutExercises);
    //     _context.UserCustomWorkout.Remove(customWorkout);
    //
    //     await _context.SaveChangesAsync();
    // }
    //
    // public async Task<UserCustomWorkoutActivity?> GetUserCustomWorkoutActivity(Guid userId, long workoutExerciseId, int set,
    //     int week, int day)
    // {
    //     return await _context.UserCustomWorkoutActivity
    //         .Where(e => e.UserId == userId)
    //         .Where(e => e.UserCustomWorkoutExerciseId == workoutExerciseId)
    //         .Where(e => e.Set == set)
    //         .Where(e => e.Week == week)
    //         .Where(e => e.Day == day)
    //         .FirstOrDefaultAsync();
    // }
    //
    // public async Task<IEnumerable<UserCustomWorkoutActivity>> GetUserCustomWorkoutActivities(Guid userId,
    //     long workoutExerciseId)
    // {
    //     return await _context.UserCustomWorkoutActivity
    //         .Where(e => e.UserId == userId)
    //         .Where(e => e.UserCustomWorkoutExerciseId == workoutExerciseId)
    //         .ToListAsync();
    // }
    //
    // public async Task DeleteUserCustomWorkoutActivities(IEnumerable<UserCustomWorkoutActivity> activities)
    // {
    //     _context.UserCustomWorkoutActivity.RemoveRange(activities);
    //
    //     await _context.SaveChangesAsync();
    // }
    //
    // public async Task AddUserCustomWorkoutActivity(UserCustomWorkoutActivity userCustomWorkoutActivity)
    // {
    //     userCustomWorkoutActivity.Created = DateTime.UtcNow;
    //     _context.UserCustomWorkoutActivity.Add(userCustomWorkoutActivity);
    //
    //     await _context.SaveChangesAsync();
    // }
    //
    // public async Task UpdateUserCustomWorkoutActivity(UserCustomWorkoutActivity userCustomWorkoutActivity)
    // {
    //     _context.UserCustomWorkoutActivity.Update(userCustomWorkoutActivity);
    //
    //     await _context.SaveChangesAsync();
    // }
    //
    // public async Task<IEnumerable<UserCustomWorkoutExercise>> GetUserCustomWorkoutExercises(long workoutExerciseId)
    // {
    //     return await _context.UserCustomWorkoutExercise
    //         .Where(e => e.UserCustomWorkoutId == workoutExerciseId)
    //         .Include(e => e.Exercise)
    //         .ToListAsync();
    // }
    //
    // public async Task<UserCustomWorkoutExercise?> GetUserCustomWorkoutExercise(long userCustomWorkoutExerciseId)
    // {
    //     var workoutExercise = await _context.UserCustomWorkoutExercise
    //         .Where(e => e.Id == userCustomWorkoutExerciseId)
    //         .Include(e => e.Exercise)
    //         .FirstOrDefaultAsync();
    //
    //     return workoutExercise;
    // }
    //
    // public async Task<long> AddUserCustomWorkoutExercise(UserCustomWorkoutExercise userCustomWorkoutExercise)
    // {
    //     _context.UserCustomWorkoutExercise.Add(userCustomWorkoutExercise);
    //
    //     await _context.SaveChangesAsync();
    //     return userCustomWorkoutExercise.Id;
    // }
    //
    // public async Task<long> UpdateUserCustomWorkoutExercise(UserCustomWorkoutExercise userCustomWorkoutExercise)
    // {
    //     _context.UserCustomWorkoutExercise.Update(userCustomWorkoutExercise);
    //
    //     await _context.SaveChangesAsync();
    //     return userCustomWorkoutExercise.Id;
    // }
}