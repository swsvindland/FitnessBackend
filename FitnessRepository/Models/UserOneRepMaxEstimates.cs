namespace FitnessRepository.Models;

public sealed class UserOneRepMaxEstimates
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    public long ExerciseId { get; set; }
    public int Estimate { get; set; }
}