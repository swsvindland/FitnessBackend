namespace FitnessRepository.Models;

public class UserHeight
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    public int Height { get; set; }
}