using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public class SupplementRepository: ISupplementRepository
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

    public async Task AddUserSupplement()
    {
        
    }
}