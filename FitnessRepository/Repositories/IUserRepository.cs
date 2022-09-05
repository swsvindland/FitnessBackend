using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IUserRepository
{
    Task<Users?> GetUserById(Guid userId);
    public Task<Users?> GetUserByEmail(string email);
}