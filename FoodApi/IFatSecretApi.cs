using FoodApi.Models;

namespace FoodApi;

public interface IFatSecretApi
{
    Task<FatSecretAuth?> AuthFatSecretApi();
    Task<IEnumerable<FatSecretSearchItem>?> SearchFoods(string query, int pageNumber, string? oldToken);
    Task<FatSecretItem?> GetFood(long foodId, string? oldToken);
    Task<long?> GetIdFromBarcode(string barcode, string? oldToken);
    Task<IEnumerable<string>?> Autocomplete(string query, string? oldToken);
}