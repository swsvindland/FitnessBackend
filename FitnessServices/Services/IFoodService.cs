using FitnessServices.Models;

namespace FitnessServices.Services;

public interface IFoodService
{
    Task<IEnumerable<Macros>> GenerateMacros(Guid userId);
    Task<IEnumerable<string>?> AutocompleteFood(string query);
}