namespace FitnessServices.Models;

public sealed class UserNextWorkout
{
    public long Id { get; set; }
    public long WorkoutId { get; set; }
    public short Week { get; set; }
    public short Day { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public bool WorkoutCompleted { get; set; }
}