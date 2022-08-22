using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public class SupplementRepository : ISupplementRepository
{
    private readonly FitnessContext _context;

    public SupplementRepository(FitnessContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Supplements>> GetAllSupplements()
    {
        return await _context.Supplements.ToListAsync();
    }

    public async Task AddUserSupplement(UserSupplement userSupplement)
    {
        _context.UserSupplements.Add(userSupplement);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserSupplement(UserSupplement userSupplement)
    {
        _context.UserSupplements.Update(userSupplement);

        await _context.SaveChangesAsync();
    }

    public async Task<UserSupplement?> GetUserSupplement(long id)
    {
        return await _context.UserSupplements.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<UserSupplement>> GetUserSupplementByUserId(Guid userId)
    {
        return await _context.UserSupplements
            .Where(e => e.UserId == userId)
            .Include(e => e.Supplement)
            .ToListAsync();
    }
}