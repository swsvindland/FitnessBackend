using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface IFoodRepository
{
    Task<UserCustomMacros?> GetUserCustomMacros(Guid userId);
    Task AddUserCustomMacros(UserCustomMacros userCustomMacros);
    Task UpdateUserCustomMacros(UserCustomMacros userCustomMacros);
    Task<IEnumerable<FoodV2>> GetAllFoodsV2();
    Task<FoodV2?> GetFoodV2ById(long id);
    Task<long> AddFoodV2(FoodV2 food);
    Task<long> UpdateFoodV2(FoodV2 food);
    Task UpdateFoodsV2(IEnumerable<FoodV2> food);
    Task AddFoodV2Servings(IEnumerable<FoodV2Servings> servings);
    Task UpdateFoodV2Servings(IEnumerable<FoodV2Servings> servings);
    Task<UserFoodV2?> GetUserFoodV2(long userFoodId);
    Task<IEnumerable<UserFoodV2>> GetAllUserFoodsV2(Guid userId);
    Task<IEnumerable<UserFoodV2>> GetAllUserFoodsV2ByDate(Guid userId, DateTime date);
    Task<long> AddUserFoodV2(UserFoodV2 userFood);
    Task UpdateUserFoodV2(UserFoodV2 userFood);
    Task DeleteUserFoodV2(long userFoodId);
}