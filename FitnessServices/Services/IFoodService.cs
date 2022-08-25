using FitnessServices.Models;

namespace FitnessServices.Services;

public interface IFoodService
{
    Task<IEnumerable<Macros>> GenerateMacros(Guid userId);
}