using FitnessRepository;
using FitnessRepository.Models;

namespace FitnessServices.Services;

public interface ISupplementService
{
    public Task<IEnumerable<Supplements>> GetAllSupplements();
}