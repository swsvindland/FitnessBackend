using System.Security.Cryptography;
using System.Text;
using FitnessRepository.Models;
using FitnessRepository.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FitnessServices.Services;

public sealed class UserService: IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<(bool, UserToken?)> AuthByEmailPassword(string email, string password)
    {
        var user = await GetUserByEmail(email);
        if (user == null)
        {
            return (false, null);
        }
        
        var hashedPassword = HashPassword(password, user.Salt);
        
        if (user.Password != hashedPassword)
        {
            return (false, null);
        }

        var token = GenerateToken();

        var userToken = new UserToken()
        {
            UserId = user.Id,
            Token = token,
            Created = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddDays(90)
        };

        await _userRepository.AddToken(userToken);
        
        return (true, userToken);
    }

    public string GenerateToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
    
    public string GenerateSalt()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(8));
    }
    
    public string HashPassword(string password, string salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: Encoding.UTF8.GetBytes(salt),
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
    }

    public async Task CreateUser(string email, string password)
    {
        var salt = GenerateSalt();
        var hashedPassword = HashPassword(password, salt);

        var user = new Users()
        {
            Id = Guid.NewGuid(),
            Email = email,
            LastLogin = DateTime.UtcNow,
            Created = DateTime.UtcNow,
            Sex = Sex.Unknown,
            Salt = salt,
            Password = hashedPassword
        };

        await _userRepository.AddUser(user);
    }
    
    public async Task<Users?> GetUserById(Guid userId)
    {
        return await _userRepository.GetUserById(userId);
    }

    public async Task<Users?> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }
    
    public async Task<UserToken?> GetToken(Guid userId, string token)
    {
        return await _userRepository.GetToken(userId, token);
    }
}