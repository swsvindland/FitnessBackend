using FitnessServices.Models;
using FoodApi.Models;

namespace FitnessServices.Services;

public interface IFoodService
{
    Task<IEnumerable<Macros>> GenerateMacros(Guid userId);
    Task<IEnumerable<string>?> AutocompleteFood(string query);
    Task<IEnumerable<EdamamFood>?> ParseFood(string foodQuery);
    Task<EdamamNutrients?> GetFoodDetails(string foodId);
}