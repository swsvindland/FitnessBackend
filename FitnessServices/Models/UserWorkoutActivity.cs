namespace FitnessServices.Models;

public sealed class UserWorkoutActivityModel
{
    public long? Id { get; set; }
    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    public long WorkoutBlockExerciseId { get; set; }
    public int Set { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; }
    public bool? Saved { get; set; }
}