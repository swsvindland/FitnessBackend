using FitnessRepository.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Repositories;

public sealed class FoodRepository: IFoodRepository
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
}