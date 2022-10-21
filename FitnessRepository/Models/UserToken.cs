namespace FitnessRepository.Models;

public class UserToken
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public DateTime Created { get; set; }
    public DateTime Expires {get; set; }
}