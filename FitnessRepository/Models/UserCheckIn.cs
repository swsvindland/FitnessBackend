namespace FitnessRepository.Models;

public class UserCheckIn
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
}