using FitnessRepository;

namespace FitnessServices.Services;

public interface ISupplementService
{
    public Task<IEnumerable<Supplements>> GetAllSupplements();
}