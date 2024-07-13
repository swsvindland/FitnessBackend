using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public sealed class FoodRepository : IFoodRepository
{
    private readonly FitnessContext _context;

    public FoodRepository(FitnessContext context)
    {
        _context = context;
    }

    public async Task<UserCustomMacros?> GetUserCustomMacros(Guid userId)
    {
        return await _context.UserCustomMacros
            .OrderBy(e => e.Created)
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task AddUserCustomMacros(UserCustomMacros userCustomMacros)
    {
        _context.UserCustomMacros.Add(userCustomMacros);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserCustomMacros(UserCustomMacros userCustomMacros)
    {
        _context.UserCustomMacros.Update(userCustomMacros);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Food>> GetAllFoodsV2()
    {
        return await _context.FoodV2
            .Include(e => e.Servings)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<Food>> RefreshAllFoodsV2()
    {
        var foods = await _context.FoodV2
            .Include(e => e.Servings)
            .ToListAsync();

        foreach (var food in foods)
        {
            food.Updated = DateTime.UtcNow;
            foreach (var serving in food.Servings)
            {
                serving.Updated = DateTime.UtcNow;
            }
        }
        
        _context.UpdateRange(foods);
        await _context.SaveChangesAsync();

        return foods.AsReadOnly();
    }

    public async Task<Food?> GetFoodV2ByExternalId(long externalId)
    {
        return await _context.FoodV2
            .Include(e => e.Servings)
            .FirstOrDefaultAsync(e => e.ExternalId == externalId);
    }
    
    public async Task<long> AddFoodV2(Food food)
    {
        _context.FoodV2.Add(food);
        await _context.SaveChangesAsync();
        return food.ExternalId;
    }
    
    public async Task<long> UpdateFoodV2(Food food)
    {
        _context.FoodV2.Update(food);
        await _context.SaveChangesAsync();
        return food.ExternalId;
    }
    
    public async Task UpdateFoodsV2(IEnumerable<Food> food)
    {
        _context.FoodV2.UpdateRange(food);
        await _context.SaveChangesAsync();
    }
    
    public async Task AddFoodV2Servings(IEnumerable<FoodServings> servings)
    {
        _context.FoodV2Servings.AddRange(servings);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateFoodV2Servings(IEnumerable<FoodServings> servings)
    {
        _context.FoodV2Servings.UpdateRange(servings);
        await _context.SaveChangesAsync();
    }

    public async Task<UserFood?> GetUserFoodV2(long userFoodId)
    {
            return await _context.UserFoodV2
            .Include(e => e.Food)
            .Include(e => e.Serving)
            .FirstOrDefaultAsync(e => e.Id == userFoodId);
    }

    public async Task<IEnumerable<UserFood>> GetAllUserFoodsV2(Guid userId)
    {
        return await _context.UserFoodV2
            .Include(e => e.Food)
            .Include(e => e.Serving)
            .OrderByDescending(e => e.Created)
            .Where(e => e.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<UserFood>> GetAllUserFoodsV2ByDate(Guid userId, DateTime date)
    {
        return await _context.UserFoodV2
            .Include(e => e.Food)
            .Include(e => e.Serving)
            .Where(e => e.UserId == userId)
            .Where(e => e.Created.Date == date.Date)
            .ToListAsync();
    }

    public async Task <long> AddUserFoodV2(UserFood userFood)
    {
        _context.UserFoodV2.Add(userFood);
        await _context.SaveChangesAsync();
        return userFood.Id;
    }
    
    public async Task UpdateUserFoodV2(UserFood userFood)
    {
        _context.UserFoodV2.Update(userFood);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUserFoodV2(long userFoodId)
    {
        var userFood = await _context.UserFoodV2.FirstOrDefaultAsync(e => e.Id == userFoodId);

        if (userFood == null)
        {
            return;
        }

        _context.UserFoodV2.Remove(userFood);
        await _context.SaveChangesAsync();
    }
}