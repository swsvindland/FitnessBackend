using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface ISupplementRepository
{
    public Task<IEnumerable<Supplements>> GetAllSupplements();
    public Task AddUserSupplement(UserSupplement userSupplement);
    public Task UpdateUserSupplement(UserSupplement userSupplement);
    Task RemoveUserSupplement(UserSupplement userSupplement);
    public Task<UserSupplement?> GetUserSupplement(long id);
    public Task<IEnumerable<UserSupplement>> GetUserSupplementByUserId(Guid userId);
    Task AddUserSupplementActivity(UserSupplementActivity userSupplementActivity);
    Task RemoveUserSupplementActivity(UserSupplementActivity userSupplementActivity);
    Task<IEnumerable<UserSupplementActivity>> GetUserSupplementActivityByUserId(Guid userId);
    Task<UserSupplementActivity?> GetUserSupplementActivity(Guid userId, long userSupplementId, DateTime date,
        SupplementTimes supplementTime);
}