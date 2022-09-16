using FitnessRepository.Models;
using FitnessRepository.Repositories;

namespace FitnessServices.Services;

public sealed class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> AuthByEmailPassword(string email, string password)
    {
        var user = await GetUserByEmail(email);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (user.Password != password)
        {
            throw new Exception("Password is incorrect");
        }

        return "Allowed";
    }
    
    public async Task<Users?> GetUserById(Guid userId)
    {
        return await _userRepository.GetUserById(userId);
    }

    public async Task<Users?> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }
}