using FitnessRepository;
using FitnessRepository.Models;
using FitnessRepository.Repositories;

namespace FitnessServices.Services;

public class SupplementService: ISupplementService
{
    private readonly ISupplementRepository _supplementRepository;

    public SupplementService(ISupplementRepository supplementRepository)
    {
        _supplementRepository = supplementRepository;
    }
    
    public async Task<IEnumerable<Supplements>> GetAllSupplements()
    {
        return await _supplementRepository.GetAllSupplements();
    }
}