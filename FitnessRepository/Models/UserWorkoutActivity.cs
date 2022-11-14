namespace FitnessRepository.Models;

public sealed class UserWorkoutActivity
{
    public long? Id { get; set; }
    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    public long WorkoutBlockExerciseId { get; set; }
    public int Set { get; set; }
    public int Reps { get; set; }
    public float Weight { get; set; }
    public int Week { get; set; }
    public int Day { get; set; }
}