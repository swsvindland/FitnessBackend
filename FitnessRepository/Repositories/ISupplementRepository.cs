using FitnessRepository.Models;

namespace FitnessRepository.Repositories;

public interface ISupplementRepository
{
    public Task<IEnumerable<Supplements>> GetAllSupplements();
}