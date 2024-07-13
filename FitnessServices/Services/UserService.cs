using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FitnessServices.Services;

public sealed class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly RandomNumberGenerator _randomNumberGenerator;
    private readonly string _sendGridApiKey;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _randomNumberGenerator = RandomNumberGenerator.Create();
        _sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY") ?? string.Empty;
    }

    public async Task<AuthResponse?> AuthByEmailPasswordV2(string email, string password)
    {
        var user = await GetUserByEmail(email);
        if (user == null)
        {
            return null;
        }
        
        var hashedPassword = HashPassword(password, user?.Salt ?? string.Empty);
        
        if (user?.Password != hashedPassword)
        {
            return null;
        }

        var token = GenerateJwt(email, user.Id);

        return new AuthResponse()
        {
            UserId = user.Id,
            Token = token
        };
    }

    private static string GenerateJwt(string email, Guid userId)
    {
        const string issuer = "https://workout-track.com";
        const string audience = "https://workout-track.com";
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET") ?? "");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddDays(90),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        return stringToken;
    }
    
    private static string GenerateSalt()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(8));
    }
    
    private static string HashPassword(string password, string salt)
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
    
    private string GenerateRandomString(int length)
    {
        var buffer = new byte[length];
        _randomNumberGenerator.GetBytes(buffer);
        return Encoding.UTF8.GetString(buffer);
    }
    
    private async Task SendForgotPasswordEmail(string email, string password)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var from = new EmailAddress("sam@workout-track.com");
        const string subject = "Workout Track - Forgot Password";
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
    
    private async Task<Users> CreateSsoUser(string email)
    {
        var user = new Users()
        {
            Id = Guid.NewGuid(),
            Email = email,
            LastLogin = DateTime.UtcNow,
            Created = DateTime.UtcNow,
            Sex = Sex.Unknown,
            UserRole = UserRole.User,
            Unit = UserUnits.Imperial,
            Paid = false,
        };

        await _userRepository.AddUser(user);

        return user;
    }
    
    public async Task UpdateLastLogin(Guid userId)
    {
        var user = await GetUserById(userId);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        user.LastLogin = DateTime.UtcNow;
        user.LoginCount++;
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
    
    public async Task UpdatePaid(Guid userId, bool paid, DateTime? paidUntil)
    {
        var user = await GetUserById(userId);
        
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        user.Paid = paid;
        user.PaidUntil = paidUntil;
        await _userRepository.UpdateUser(user);
    }
    
    public async Task<Users?> GetUserById(Guid userId)
    {
        return await _userRepository.GetUserById(userId);
    }

    private async Task<Users?> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }

    public async Task DeleteOldUsers()
    {
        await _userRepository.DeleteOldUsers();
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