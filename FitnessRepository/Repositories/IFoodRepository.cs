using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IFoodRepository
{
    Task<UserCustomMacros?> GetUserCustomMacros(Guid userId);
    Task AddUserCustomMacros(UserCustomMacros userCustomMacros);
    Task UpdateUserCustomMacros(UserCustomMacros userCustomMacros);
    Task<IEnumerable<Food>> GetAllFoodsV2();
    Task<IEnumerable<Food>> RefreshAllFoodsV2();
    Task<Food?> GetFoodV2ById(long id);
    Task<long> AddFoodV2(Food food);
    Task<long> UpdateFoodV2(Food food);
    Task UpdateFoodsV2(IEnumerable<Food> food);
    Task AddFoodV2Servings(IEnumerable<FoodServings> servings);
    Task UpdateFoodV2Servings(IEnumerable<FoodServings> servings);
    Task<UserFood?> GetUserFoodV2(long userFoodId);
    Task<IEnumerable<UserFood>> GetAllUserFoodsV2(Guid userId);
    Task<IEnumerable<UserFood>> GetAllUserFoodsV2ByDate(Guid userId, DateTime date);
    Task<long> AddUserFoodV2(UserFood userFood);
    Task UpdateUserFoodV2(UserFood userFood);
    Task DeleteUserFoodV2(long userFoodId);
}