﻿using FitnessRepository.Models;
using FitnessRepository.Repositories;
using FitnessServices.Models;

namespace FitnessServices.Services;

public sealed class SupplementService: ISupplementService
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

    public async Task<IEnumerable<UserSupplementModel>> GetUserSupplements(Guid userId)
    {
        var userSupplement =  await _supplementRepository.GetUserSupplementByUserId(userId);

        return userSupplement.Select(e => new UserSupplementModel()
        {
            Id = e.Id,
            SupplementId = e.SupplementId,
            Supplement = e.Supplement,
            UserId = e.UserId,
            Created = e.Created,
            Times = e.Times.Split(","),
        });
    }

    public async Task UpdateUserSupplement(UpdateUserSupplement updateUserSupplement)
    {
        var entity = new UserSupplement()
        {
            Id = updateUserSupplement.Id,
            Created = DateTime.UtcNow,
            UserId = updateUserSupplement.UserId,
            SupplementId = updateUserSupplement.SupplementId,
            Times = string.Join(",",  updateUserSupplement.Times)
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
        var today = DateTime.Parse(date);

        return await _supplementRepository.GetUserSupplementActivity(userId, userSupplementId, today, time);
    }
    
    public async Task ToggleUserSupplementActivity(UpdateUserSupplementActivity updateUserSupplementActivity)
    {
        var today = DateTime.Parse(updateUserSupplementActivity.Date).Date;
        
        var entity =
            await _supplementRepository.GetUserSupplementActivity(
                updateUserSupplementActivity.UserId, updateUserSupplementActivity.UserSupplementId, today, updateUserSupplementActivity.Time);
        
        if (entity == null)
        {
            var newEntity = new UserSupplementActivity()
            {
                Updated = today,
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
    
    public async Task<IEnumerable<UserSupplementActivity>> GetUserSupplementActivitiesByDate(Guid userId, DateTime date)
    {
        return await _supplementRepository.GetUserSupplementActivitiesByDate(userId, date);
    }
}