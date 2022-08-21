namespace FitnessRepository;

public interface ISupplementRepository
{
    public Task<IEnumerable<Supplements>> GetAllSupplements();
}