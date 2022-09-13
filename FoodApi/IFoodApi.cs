using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodApi;

public interface IFoodApi
{
    Task<IEnumerable<string>?> AutocompleteFood(string query);
}