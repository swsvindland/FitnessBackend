namespace FitnessRepository.Models;

public sealed class UserWorkout
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    public long WorkoutId { get; set; }
    public bool Active { get; set; }
}