namespace FitnessRepository.Models;

public class WorkoutBlockExercise
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public long WorkoutBlockId { get; set; }
    public long ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    public int Day { get; set; }
    public int Sets { get; set; }
    public int MinReps { get; set; }
    public int MaxReps { get; set; }
}