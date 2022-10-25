using FoodApi.Models;

namespace FoodApi;

public interface IFoodApi
{
    Task<IEnumerable<EdamamFoodHint>?> ParseFood(string foodQuery, string? barcode);
    Task<EdamamNutrients?> Nutrients(string foodId, float servingSizeInGrams);
    Task<IEnumerable<string>?> AutocompleteFood(string query);
}