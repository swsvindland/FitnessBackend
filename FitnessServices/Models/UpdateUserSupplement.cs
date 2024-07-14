namespace FitnessServices.Models;

public sealed class UpdateUserSupplement
{
    public long? Id { get; set; }
    public Guid UserId { get; set; }
    public long SupplementId { get; set; }
    public List<string> Times { get; set; } = [];
}