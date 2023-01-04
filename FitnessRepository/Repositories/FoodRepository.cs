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


    public async Task<long> AddFood(Food food)
    {
        _context.Food.Add(food);
        await _context.SaveChangesAsync();
        return food.Id;
    }

    public async Task<Food?> GetFood(long foodId)
    {
        return await _context.Food.FirstOrDefaultAsync(e => e.Id == foodId);
    }

    public async Task<Food?> GetFoodByEdamamId(string edamamFoodId)
    {
        return await _context.Food.FirstOrDefaultAsync(e => e.EdamamFoodId == edamamFoodId);
    }

    public async Task<IEnumerable<Food>> GetAllFoods()
    {
        return await _context.Food.ToListAsync();
    }

    public async Task AddUserFood(UserFood userFood)
    {
        _context.UserFood.Add(userFood);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserFood(UserFood userFood)
    {
        _context.UserFood.Update(userFood);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserFood>> GetUserFoods(Guid userId, DateTime date)
    {
        return await _context.UserFood
            .Where(f => f.UserId == userId)
            .Where(e => e.Created.Date == date.Date)
            .Include(f => f.Food)
            .ToListAsync();
    }

    public async Task<UserFood?> GetUserFood(Guid userId, DateTime date, long foodId)
    {
        return await _context.UserFood
            .Where(e => e.FoodId == foodId)
            .Where(e => e.UserId == userId)
            .Where(e => e.Created.Date == date.Date)
            .Include(f => f.Food)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteUserFood(long userFoodId)
    {
        var userFood = await _context.UserFood.FirstOrDefaultAsync(e => e.Id == userFoodId);

        if (userFood == null)
        {
            return;
        }

        _context.UserFood.Remove(userFood);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<FoodV2>> GetAllFoodsV2()
    {
        return await _context.FoodV2
            .Include(e => e.Servings)
            .ToListAsync();
    }

    public async Task<FoodV2?> GetFoodV2ById(long id)
    {
        return await _context.FoodV2
            .Include(e => e.Servings)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task<long> AddFoodV2(FoodV2 food)
    {
        _context.FoodV2.Add(food);
        await _context.SaveChangesAsync();
        return food.Id;
    }
    
    public async Task<long> UpdateFoodV2(FoodV2 food)
    {
        _context.FoodV2.Update(food);
        await _context.SaveChangesAsync();
        return food.Id;
    }
    
    public async Task AddFoodV2Servings(IEnumerable<FoodV2Servings> servings)
    {
        _context.FoodV2Servings.AddRange(servings);
        await _context.SaveChangesAsync();
    }
    
    public async Task UpdateFoodV2Servings(IEnumerable<FoodV2Servings> servings)
    {
        _context.FoodV2Servings.UpdateRange(servings);
        await _context.SaveChangesAsync();
    }

    public async Task<UserFoodV2?> GetUserFoodV2(long userFoodId)
    {
            return await _context.UserFoodV2
            .Include(e => e.FoodV2)
            .Include(e => e.Serving)
            .FirstOrDefaultAsync(e => e.Id == userFoodId);
    }

    public async Task<IEnumerable<UserFoodV2>> GetAllUserFoodsV2(Guid userId)
    {
        return await _context.UserFoodV2
            .Include(e => e.FoodV2)
            .Include(e => e.Serving)
            .OrderByDescending(e => e.Created)
            .Where(e => e.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<IEnumerable<UserFoodV2>> GetAllUserFoodsV2ByDate(Guid userId, DateTime date)
    {
        return await _context.UserFoodV2
            .Include(e => e.FoodV2)
            .Include(e => e.Serving)
            .Where(e => e.UserId == userId)
            .Where(e => e.Created.Date == date.Date)
            .ToListAsync();
    }

    public async Task <long> AddUserFoodV2(UserFoodV2 userFood)
    {
        _context.UserFoodV2.Add(userFood);
        await _context.SaveChangesAsync();
        return userFood.Id;
    }
    
    public async Task UpdateUserFoodV2(UserFoodV2 userFood)
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