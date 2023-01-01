using FitnessRepository.Models;
using FitnessServices.Models;
using FoodApi.Models;

namespace FitnessServices.Services;

public interface IFoodService
{
    Task<Macros?> GetUserMacros(Guid userId);
    Task<IEnumerable<Macros>> GenerateMacros(Guid userId);
    Task AddUserCustomMacros(Guid userId, Macros macros);
    Task<IEnumerable<string>?> AutocompleteFood(string query);
    Task<IEnumerable<EdamamFoodHint>?> ParseFood(string foodQuery, string? barcode);
    Task<IEnumerable<FatSecretSearchItem>> SearchFood(string query, int page);
    Task<FoodV2> GetFoodById(long foodId);
    Task<EdamamNutrients?> GetFoodDetails(string foodId, float servingSizeInGrams);
    Task<Macros> GetUserCurrentMacos(Guid userId, DateTime date);
    Task<IEnumerable<UserFood>> GetUserFoods(Guid userId, DateTime date);
    Task<Food?> GetFood(long foodId);
    Task<UserFood?> GetUserFood(Guid userId, DateTime date, long foodId);
    Task<IEnumerable<UserFoodGridItem>> GetUserFoodsForGrid(Guid userId, DateTime date);
    Task AddUserFood(UserFood userFood);
    Task UpdateUserFood(UserFood userFood);
    Task DeleteUserFood(long userFoodId);
    Task<UserFoodV2?> GetUserFoodV2(long userFoodId);
    Task<IEnumerable<UserFoodV2>> GetAllUserFoodsV2ByDate(Guid userId, DateTime date);
    Task<long> AddUserFoodV2(UserFoodV2 userFood);
    Task UpdateUserFoodV2(UserFoodV2 userFood);
    Task DeleteUserFoodV2(long userFoodId);
    Task<Macros> GetUserCurrentMacosV2(Guid userId, DateTime date);
}