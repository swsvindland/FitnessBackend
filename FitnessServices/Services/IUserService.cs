using FitnessRepository.Models;

namespace FitnessServices.Services;

public interface IUserService
{
    Task<Users?> GetUserById(Guid userId);
    public Task<Users?> GetUserByEmail(string email);
}