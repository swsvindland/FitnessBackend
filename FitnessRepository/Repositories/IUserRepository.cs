using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IUserRepository
{
    public Task<Users?> GetUserByEmail(string email);
}