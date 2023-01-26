using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public sealed class BodyRepository: IBodyRepository
{
    private readonly FitnessContext _context;

    public BodyRepository(FitnessContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserWeight>> GetAllUserWeights(Guid userId)
    {
        return await _context.UserWeight.Where(e => e.UserId == userId).OrderBy(e => e.Created).ToListAsync();
    }
    
    public async Task AddUserWeight(UserWeight userWeight)
    {
        var updated = userWeight;
        updated.Created = DateTime.UtcNow;
        _context.UserWeight.Add(updated);

        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateUserWeight(UserWeight userWeight)
    {
        _context.UserWeight.Update(userWeight);

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUserWeight(long id)
    {
        var userWeight = await _context.UserWeight.FirstOrDefaultAsync(e => e.Id == id);
        _context.UserWeight.Remove(userWeight);

        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUserWeight(UserWeight userWeight)
    {
        _context.UserWeight.Remove(userWeight);

        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<UserBody>> GetAllUserBodies(Guid userId)
    {
        return await _context.UserBody.Where(e => e.UserId == userId).OrderBy(e => e.Created).ToListAsync();
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
        return await _context.UserBloodPressure.Where(e => e.UserId == userId).OrderBy(e => e.Created).ToListAsync();
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

    public async Task AddUserHeight(UserHeight userHeight)
    {
        _context.UserHeight.Add(userHeight);

        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<UserHeight>> GetUserHeights(Guid userId)
    {
        return await _context.UserHeight.Where(e => e.UserId == userId).OrderBy(e => e.Created).ToListAsync();
    }
    
    public async Task<IEnumerable<ProgressPhoto>> GetProgressPhotos(Guid userId)
    {
        return await _context.ProgressPhoto
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Created)
            .ToListAsync();
    }

    public async Task<long> AddProgressPhoto(ProgressPhoto progressPhoto)
    {
        _context.ProgressPhoto.Add(progressPhoto);

        await _context.SaveChangesAsync();

        return progressPhoto.Id;
    }
    
    public async Task<long> UpdateProgressPhoto(ProgressPhoto progressPhoto)
    {
        _context.ProgressPhoto.Update(progressPhoto);

        await _context.SaveChangesAsync();

        return progressPhoto.Id;
    }
    
    public async Task DeleteProgressPhoto(long id)
    {
        var progressPhoto = await _context.ProgressPhoto.FirstOrDefaultAsync(e => e.Id == id);
        _context.ProgressPhoto.Remove(progressPhoto);

        await _context.SaveChangesAsync();
    }
}