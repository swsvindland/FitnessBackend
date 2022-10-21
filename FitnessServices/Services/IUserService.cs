using FitnessRepository.Models;

namespace FitnessServices.Services;

public interface IUserService
{
    Task<(bool, UserToken?)> AuthByEmailPassword(string email, string password);
    Task<Users?> GetUserById(Guid userId);
    public Task<Users?> GetUserByEmail(string email);
    Task CreateUser(string email, string password);
    Task<UserToken?> GetTokenByUserId(Guid userId);
}