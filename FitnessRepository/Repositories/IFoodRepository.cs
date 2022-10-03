using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IFoodRepository
{
    Task AddFood(Food food);
    Task<Food?> GetFood(long foodId);
    Task<IEnumerable<Food>> GetAllFoods();
    Task AddUserFood(UserFood userFood);
    Task UpdateUserFood(UserFood userFood);
    Task<IEnumerable<UserFood>> GetUserFoods(Guid userId);
}