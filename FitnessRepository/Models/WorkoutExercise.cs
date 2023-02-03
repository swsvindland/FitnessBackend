namespace FitnessRepository.Models;

public sealed class WorkoutExercise
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public long WorkoutId { get; set; }
    public long ExerciseId { get; set; }
    public Exercise? Exercise { get; set; }
    public int Day { get; set; }
    public int Sets { get; set; }
    public int? MinReps { get; set; }
    public int? MaxReps { get; set; }
    public int? Time { get; set; }
    public int? Order { get; set; }
    public int? RestTime { get; set; }
}