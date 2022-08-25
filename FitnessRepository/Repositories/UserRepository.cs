using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public class UserRepository: IUserRepository
{
    private readonly FitnessContext _context;

    public UserRepository(FitnessContext context)
    {
        _context = context;
    }
    
    public async Task<Users?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.Email == email.ToLower());
    }
}