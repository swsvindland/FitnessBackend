namespace FitnessServices.Models;

public sealed class UserBodyFat
{
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public double BodyFat { get; set; }
}