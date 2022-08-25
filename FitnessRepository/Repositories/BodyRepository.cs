using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public class BodyRepository: IBodyRepository
{
    private readonly FitnessContext _context;

    public BodyRepository(FitnessContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserWeight>> GetAllUserWeights(Guid userId)
    {
        return await _context.UserWeight.Where(e => e.UserId == userId).ToListAsync();
    }
    
    public async Task AddUserWeight(UserWeight userWeight)
    {
        var updated = userWeight;
        updated.Created = DateTime.UtcNow;
        _context.UserWeight.Add(updated);

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUserWeight(UserWeight userWeight)
    {
        _context.UserWeight.Remove(userWeight);

        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<UserBody>> GetAllUserBodies(Guid userId)
    {
        return await _context.UserBody.Where(e => e.UserId == userId).ToListAsync();
    }
    
    public async Task AddUserBody(UserBody userBody)
    {
        var updated = userBody;
        updated.Created = DateTime.UtcNow;
        _context.UserBody.Add(updated);

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUserBody(UserBody userBody)
    {
        _context.UserBody.Remove(userBody);

        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<UserBloodPressure>> GetAllUserBloodPressures(Guid userId)
    {
        return await _context.UserBloodPressure.Where(e => e.UserId == userId).ToListAsync();
    }
    
    public async Task AddUserBloodPressure(UserBloodPressure userBloodPressure)
    {
        var updated = userBloodPressure;
        updated.Created = DateTime.UtcNow;
        _context.UserBloodPressure.Add(updated);

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUserBloodPressure(UserBloodPressure userBloodPressure)
    {
        _context.UserBloodPressure.Remove(userBloodPressure);

        await _context.SaveChangesAsync();
    }
}