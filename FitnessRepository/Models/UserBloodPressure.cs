namespace FitnessRepository.Models;

public sealed class UserBloodPressure
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public int Systolic { get; set; }
    public int Diastolic { get; set; }
}