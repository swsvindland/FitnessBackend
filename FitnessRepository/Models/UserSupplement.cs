namespace FitnessRepository.Models;

public sealed class UserSupplement
{
    public long? Id { get; set; }
    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    public long SupplementId { get; set; }
    public Supplements Supplement { get; set; }
    public string Times { get; set; }
}