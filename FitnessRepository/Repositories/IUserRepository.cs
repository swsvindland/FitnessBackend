using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IUserRepository
{
    Task<Users?> GetUserById(Guid userId);
    Task<IEnumerable<Users>> GetUsers();
    public Task<Users?> GetUserByEmail(string email);
    public Task AddUser(Users user);
    public Task UpdateUser(Users user);
    Task AddToken(UserToken token);
    Task<UserToken?> GetToken(Guid userId, string token);
    Task DeleteUser(Guid userId);
}