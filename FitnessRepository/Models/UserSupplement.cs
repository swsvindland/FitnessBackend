namespace FitnessRepository.Models;

public class UserSupplement
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long SupplementId { get; set; }
    public Supplements Supplement { get; set; }
}