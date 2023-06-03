using FitnessRepository.Models;
using FitnessServices.Models;

namespace FitnessServices.Services;

public interface IUserService
{
    Task<AuthResponse?> AuthByEmailPasswordV2(string email, string password);
    Task<AuthResponse?> SsoAuth(string email, string token);
    Task UpdateLastLogin(Guid userId);
    Task CheckIfPaidUntilValid(Guid userId);
    Task UpdatePaid(Guid userId, bool paid, DateTime? paidUntil);
    Task<Users?> GetUserById(Guid userId);
    Task CreateUser(string email, string password);
    Task DeleteUser(Guid userId);
    Task DeleteOldUsers();
    Task UpdateUserSex(Guid userId, Sex sex);
    Task UpdateUserUnits(Guid userId, UserUnits unit);
    Task ChangePassword(Guid userId, string oldPassword, string newPassword);
    Task ForgotPassword(string email);
}