using FitnessRepository.Models;

namespace FitnessServices.Services;

public interface IUserService
{
    public Task<Users?> GetUserByEmail(string email);
}