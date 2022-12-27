using FoodApi.Models;

namespace FoodApi;

public interface IFatSecretApi
{
    Task<IEnumerable<FatSecretSearchItem>> SearchFoods(string query, int pageNumber);
}