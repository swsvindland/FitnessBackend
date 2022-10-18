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
}