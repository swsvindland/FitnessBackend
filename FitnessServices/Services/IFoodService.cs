using FitnessRepository.Models;
using FitnessServices.Models;
using FoodApi.Models;
using Microsoft.Extensions.Logging;

namespace FitnessServices.Services;

public interface IFoodService
{
    Task<Macros?> GetUserMacros(Guid userId);
    Task<IEnumerable<Macros>> GenerateMacros(Guid userId);
    Task AddUserCustomMacros(Guid userId, Macros macros);
    Task<IEnumerable<string>?> AutocompleteFood(string query);
    Task<IEnumerable<FatSecretSearchItem>> SearchFood(string query, int page);
    Task<FoodV2> GetFoodById(long foodId);
    Task<UserFoodV2?> GetUserFoodV2(long userFoodId);
    Task<IEnumerable<UserFoodV2>> GetAllUserFoodsV2ByDate(Guid userId, DateTime date);
    Task<float> QuickAddUserFoodV2(Guid userId, long foodId, DateTime date);
    Task<float> QuickRemoveUserFoodV2(Guid userId, long foodId, DateTime date);
    Task<long> AddUserFoodV2(UserFoodV2 userFood, DateTime date);
    Task UpdateUserFoodV2(UserFoodV2 userFood);
    Task DeleteUserFoodV2(long userFoodId);
    Task<Macros> GetUserCurrentMacosV2(Guid userId, DateTime date);
    Task RefreshCashedFoodDb(ILogger logger);
    Task<FoodV2> GetFoodByBarcode(string barcode);
    Task<IEnumerable<UserFoodV2>> GetRecentUserFoods(Guid userId, DateTime date);
}