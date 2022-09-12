namespace FitnessRepository.Models;

public sealed class UserWeight
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public float Weight { get; set; }
}