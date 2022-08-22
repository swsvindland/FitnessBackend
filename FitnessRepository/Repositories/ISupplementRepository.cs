using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface ISupplementRepository
{
    public Task<IEnumerable<Supplements>> GetAllSupplements();
    public Task AddUserSupplement(UserSupplement userSupplement);
    public Task UpdateUserSupplement(UserSupplement userSupplement);
    public Task<UserSupplement?> GetUserSupplement(long id);
    public Task<IEnumerable<UserSupplement>> GetUserSupplementByUserId(Guid userId);
}