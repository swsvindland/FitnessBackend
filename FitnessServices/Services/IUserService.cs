using FitnessRepository.Models;

namespace FitnessServices.Services;

public interface IUserService
{
    Task<(bool, UserToken?)> AuthByEmailPassword(string email, string password);
    Task UpdateLastLogin(Guid userId);
    Task CheckIfPaidUntilValid(Guid userId);
    Task UpdatePaid(Guid userId, bool paid, DateTime? paidUntil);
    Task<Users?> GetUserById(Guid userId);
    Task CreateUser(string email, string password);
    Task<UserToken?> GetToken(Guid userId, string token);
    Task DeleteUser(Guid userId);
    Task UpdateUserSex(Guid userId, Sex sex);
    Task UpdateUserUnits(Guid userId, UserUnits unit);
    Task ChangePassword(Guid userId, string oldPassword, string newPassword);
    Task ForgotPassword(string email);
}