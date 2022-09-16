using FitnessRepository.Models;

namespace FitnessServices.Services;

public interface IUserService
{
    Task<string> AuthByEmailPassword(string email, string password);
    Task<Users?> GetUserById(Guid userId);
    public Task<Users?> GetUserByEmail(string email);
}