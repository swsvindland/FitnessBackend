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
        else if (entity.Times.Any() == false)
        {
            await _supplementRepository.RemoveUserSupplement(entity);
        }
        else
        {
            await _supplementRepository.UpdateUserSupplement(entity);
        }
    }
    
    public async Task<UserSupplementActivity?> GetUserSupplementActivity(Guid userId, long userSupplementId, string date, SupplementTimes time)
    {
        var today = DateTime.Parse(date).ToUniversalTime().Date;

        return await _supplementRepository.GetUserSupplementActivity(userId, userSupplementId, today.ToUniversalTime(), time);
    }
    
    public async Task ToggleUserSupplementActivity(UpdateUserSupplementActivity updateUserSupplementActivity)
    {
        var today = DateTime.Parse(updateUserSupplementActivity.Date).ToUniversalTime().Date;
        
        var entity =
            await _supplementRepository.GetUserSupplementActivity(
                updateUserSupplementActivity.UserId, updateUserSupplementActivity.UserSupplementId, today.ToUniversalTime(), updateUserSupplementActivity.Time);
        
        if (entity == null)
        {
            var newEntity = new UserSupplementActivity()
            {
                Updated = today.ToUniversalTime(),
                UserId = updateUserSupplementActivity.UserId,
                UserSupplementId = updateUserSupplementActivity.UserSupplementId,
                Time = updateUserSupplementActivity.Time
            };
            
            await _supplementRepository.AddUserSupplementActivity(newEntity);
        }
        else
        {
            await _supplementRepository.RemoveUserSupplementActivity(entity);
        }
    }
}