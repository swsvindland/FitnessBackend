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
    Task<IEnumerable<string>?> AutocompleteFood(string query, string? oldToken);
    Task<IEnumerable<FatSecretSearchItem>?> SearchFood(string query, int page, string? oldToken);
    Task<FoodV2?> GetFoodById(long foodId, string? oldToken);
    Task<UserFoodV2?> GetUserFoodV2(long userFoodId);
    Task<IEnumerable<UserFoodV2>> GetAllUserFoodsV2ByDate(Guid userId, DateTime date);
    Task<float> QuickAddUserFoodV2(Guid userId, long foodId, DateTime date, string? oldToken);
    Task<float> QuickRemoveUserFoodV2(Guid userId, long foodId, DateTime date, string? oldToken);
    Task<long> AddUserFoodV2(UserFoodV2 userFood, DateTime date);
    Task UpdateUserFoodV2(UserFoodV2 userFood);
    Task DeleteUserFoodV2(long userFoodId);
    Task<Macros> GetUserCurrentMacosV2(Guid userId, DateTime date);
    Task RefreshCashedFoodDb(ILogger logger);
    Task<FoodV2?> GetFoodByBarcode(string barcode, string? oldToken);
    Task<IEnumerable<UserFoodV2>> GetRecentUserFoods(Guid userId, DateTime date);
    Task<FatSecretAuth?> AuthFatSecretApi();
}