using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IFoodRepository
{
    Task<long> AddFood(Food food);
    Task<Food?> GetFoodByEdamamId(string edamamFoodId);
    Task<Food?> GetFood(long foodId);
    Task<IEnumerable<Food>> GetAllFoods();
    Task AddUserFood(UserFood userFood);
    Task UpdateUserFood(UserFood userFood);
    Task<IEnumerable<UserFood>> GetUserFoods(Guid userId);
}