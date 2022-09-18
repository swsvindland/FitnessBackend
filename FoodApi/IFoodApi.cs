using FoodApi.Models;

namespace FoodApi;

public interface IFoodApi
{
    Task<IEnumerable<EdamamFood>?> ParseFood(string foodQuery);
    Task<EdamamNutrients?> Nutrients(string foodId);
    Task<IEnumerable<string>?> AutocompleteFood(string query);
}