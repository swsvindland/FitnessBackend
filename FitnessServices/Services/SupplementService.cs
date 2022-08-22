using FitnessRepository;
using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;

namespace FitnessServices.Services;

public class SupplementService: ISupplementService
{
    private readonly ISupplementRepository _supplementRepository;

    public SupplementService(ISupplementRepository supplementRepository)
    {
        _supplementRepository = supplementRepository;
    }
    
    public async Task<IEnumerable<Supplements>> GetAllSupplements()
    {
        return await _supplementRepository.GetAllSupplements();
    }

    public async Task<IEnumerable<UserSupplement>> GetUserSupplements(Guid userId)
    {
        return await _supplementRepository.GetUserSupplementByUserId(userId);
    }

    public async Task UpdateUserSupplement(UpdateUserSupplement updateUserSupplement)
    {
        var entity = new UserSupplement()
        {
            Id = updateUserSupplement.Id,
            UserId = updateUserSupplement.UserId,
            SupplementId = updateUserSupplement.SupplementId,
            Times = updateUserSupplement.Times
        };
        
        if (entity.Id == null)
        {
            await _supplementRepository.AddUserSupplement(entity);
        }
        else
        {
            await _supplementRepository.UpdateUserSupplement(entity);
        }
    }
}