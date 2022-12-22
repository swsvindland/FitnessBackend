using System.Net;
using System.Security.Cryptography;
using System.Text;
using FitnessRepository.Models;
using FitnessRepository.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using SendGrid;
using SendGrid.Helpers.Mail;

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

    public async Task ChangePassword(Guid userId, string oldPassword, string newPassword)
    {
        var user = await GetUserById(userId);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }

        var oldHashedPassword = HashPassword(oldPassword, user.Salt);
        if (oldHashedPassword != user.Password)
        {
            throw new Exception("Old password is incorrect");
        }
        
        var salt = GenerateSalt();
        var hashedPassword = HashPassword(newPassword, salt);

        user.Salt = salt;
        user.Password = hashedPassword;
        
        await _userRepository.UpdateUser(user);
    }
    
    public async Task ForgotPassword(string email)
    {
        var user = await GetUserByEmail(email);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        var newPassword = GenerateRandomString(8);
        
        var salt = GenerateSalt();
        var hashedPassword = HashPassword(newPassword, salt);
        
        await SendForgotPasswordEmail(email, newPassword);

        user.Salt = salt;
        user.Password = hashedPassword;
        
        await _userRepository.UpdateUser(user);
    }
    
    private static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
    
    private static async Task SendForgotPasswordEmail(string email, string password)
    {
        var apiKey = "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";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("sam@workout-track.com");
        var subject = "Workout Track - Forgot Password";
        var to = new EmailAddress(email);
        var plainTextContent = $"Your new password is {password}. Remember to change it after you login. You can do so by clicking on the user icon in the top right, and then the change password button.";
        var htmlContent = $"<p>Your new password is <strong>{password}</strong>. Remember to change it after you login. You can do so by clicking on the user icon in the top right, and then the change password button.</p>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        await client.SendEmailAsync(msg);
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
            Password = hashedPassword,
            UserRole = UserRole.User,
            Unit = UserUnits.Imperial,
            Paid = false,
        };

        await _userRepository.AddUser(user);
    }
    
    public async Task UpdateLastLogin(Guid userId)
    {
        var user = await GetUserById(userId);
        user.LastLogin = DateTime.UtcNow;
        await _userRepository.UpdateUser(user);
    }
    
    public async Task CheckIfPaidUntilValid(Guid userId)
    {
        var user = await GetUserById(userId);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (user.PaidUntil == null || user.PaidUntil < DateTime.UtcNow)
        {
            user.Paid = false;
        }
        await _userRepository.UpdateUser(user);
    }
    
    public async Task UpdatePaid(Guid userId, bool paid, DateTime paidUntil)
    {
        var user = await GetUserById(userId);
        user.Paid = paid;
        user.PaidUntil = paidUntil;
        await _userRepository.UpdateUser(user);
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

    public async Task DeleteUser(Guid userId)
    {
        await _userRepository.DeleteUser(userId);
    }

    public async Task UpdateUserSex(Guid userId, Sex sex)
    {
        var user = await _userRepository.GetUserById(userId);

        if (user == null)
        {
            return;
        }

        user.Sex = sex;
        await _userRepository.UpdateUser(user);
    }
    
    public async Task UpdateUserUnits(Guid userId, UserUnits unit)
    {
        var user = await _userRepository.GetUserById(userId);

        if (user == null)
        {
            return;
        }

        user.Unit = unit;
        await _userRepository.UpdateUser(user);
    }
}