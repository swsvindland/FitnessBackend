namespace FitnessServices.Models;

public sealed class UserWorkoutActivityModel
{
    public long? Id { get; set; }
    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    public long WorkoutExerciseId { get; set; }
    public int Set { get; set; }
    public int? Reps { get; set; }
    public int? Time { get; set; }
    public float? Weight { get; set; }
    public bool? Saved { get; set; }
}