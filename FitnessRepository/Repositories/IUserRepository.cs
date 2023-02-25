using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IUserRepository
{
    Task<Users?> GetUserById(Guid userId);
    public Task<Users?> GetUserByEmail(string email);
    public Task AddUser(Users user);
    public Task UpdateUser(Users user);
    Task DeleteUser(Guid userId);
}