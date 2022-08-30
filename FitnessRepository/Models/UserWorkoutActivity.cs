namespace FitnessRepository.Models;

public class UserWorkoutActivity
{
    public long? Id { get; set; }
    public DateTime Created { get; set; }
    public Guid UserId { get; set; }
    public long WorkoutBlockExerciseId { get; set; }
    public int Set { get; set; }
    public int Reps { get; set; }
    public int Weight { get; set; }
}