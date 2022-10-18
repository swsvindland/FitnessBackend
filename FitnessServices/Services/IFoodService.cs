using FitnessRepository.Models;
using FitnessServices.Models;
using FoodApi.Models;

namespace FitnessServices.Services;

public interface IFoodService
{
    Task<IEnumerable<Macros>> GenerateMacros(Guid userId);
    Task<IEnumerable<string>?> AutocompleteFood(string query);
    Task<IEnumerable<EdamamFood>?> ParseFood(string foodQuery, string? barcode);
    Task<EdamamNutrients?> GetFoodDetails(string foodId);
    Task<Macros> GetUserCurrentMacos(Guid userId, DateTime date);
    Task<IEnumerable<UserFood>> GetUserFoods(Guid userId, DateTime date);
    Task<IEnumerable<UserFoodGridItem>> GetUserFoodsForGrid(Guid userId, DateTime date);
    Task AddUserFood(UserFood userFood);
}