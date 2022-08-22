using FitnessRepository.Models;
using FitnessServices.Models;

namespace FitnessServices.Services;

public interface ISupplementService
{
    public Task<IEnumerable<Supplements>> GetAllSupplements();
    Task<IEnumerable<UserSupplement>> GetUserSupplements(Guid userId);
    public Task UpdateUserSupplement(UpdateUserSupplement updateUserSupplement);
}