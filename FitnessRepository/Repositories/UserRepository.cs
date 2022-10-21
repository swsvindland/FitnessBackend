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
        return await _context.Users.FirstOrDefaultAsync(e => e.Id == userId);
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

    public async Task AddToken(UserToken token)
    {
        _context.UserToken.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task<UserToken?> GetTokenByUserId(Guid userId)
    {
        return await _context.UserToken
            .OrderBy(e => e.Created)
            .LastOrDefaultAsync(e => e.UserId == userId);
    }
}