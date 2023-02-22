namespace FitnessRepository.Models;

public class UserWorkoutSubstitution
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long WorkoutExerciseId { get; set; }
    public long ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}