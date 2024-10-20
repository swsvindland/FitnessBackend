using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly FitnessContext _context;

    public UserRepository(FitnessContext context)
    {
        _context = context;
    }

    public async Task<Users?> GetUserById(Guid userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    public async Task<Users?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.Email == email.ToLower());
    }

    public async Task AddUser(Users user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(Users user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(Guid userId)
    {
        _context.UserWorkoutsCompleted.RemoveRange(_context.UserWorkoutsCompleted.Where(e => e.UserId == userId));
        _context.UserWorkoutActivity.RemoveRange(_context.UserWorkoutActivity.Where(e => e.UserId == userId));
        _context.UserWorkout.RemoveRange(_context.UserWorkout.Where(e => e.UserId == userId));
        _context.Users.RemoveRange(_context.Users.Where(e => e.Id == userId));
        _context.UserWeight.RemoveRange(_context.UserWeight.Where(e => e.UserId == userId));
        _context.UserSupplements.RemoveRange(_context.UserSupplements.Where(e => e.UserId == userId));
        _context.UserSupplementActivity.RemoveRange(_context.UserSupplementActivity.Where(e => e.UserId == userId));
        _context.UserOneRepMaxEstimates.RemoveRange(_context.UserOneRepMaxEstimates.Where(e => e.UserId == userId));
        _context.UserHeight.RemoveRange(_context.UserHeight.Where(e => e.UserId == userId));
        _context.UserBody.RemoveRange(_context.UserBody.Where(e => e.UserId == userId));
        _context.UserBloodPressure.RemoveRange(_context.UserBloodPressure.Where(e => e.UserId == userId));
        _context.UserCustomMacros.RemoveRange(_context.UserCustomMacros.Where(e => e.UserId == userId));
        _context.ProgressPhoto.RemoveRange(_context.ProgressPhoto.Where(e => e.UserId == userId));
        _context.UserFoodV2.RemoveRange(_context.UserFoodV2.Where(e => e.UserId == userId));
        _context.UserWorkoutSubstitution.RemoveRange(_context.UserWorkoutSubstitution.Where(e => e.UserId == userId));
        
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> DeleteOldUsers()
    {
        var users = await _context.Users.Where(e => e.LastLogin < DateTime.Now.AddDays(-90)).ToListAsync();

        var ids = users.Select(e => e.Id).ToArray();

        for (var i = 0; i < ids.Length; ++i)
        {
            await DeleteUser(ids[i]);
        }

        return true;
    }
}