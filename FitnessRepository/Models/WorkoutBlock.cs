namespace FitnessRepository.Models;

public sealed class WorkoutBlock
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public long WorkoutId { get; set; }
    public int BlockIndex { get; set; }
    public int Duration { get; set; }
    public int Days { get; set; }
    public IEnumerable<WorkoutBlockExercise>? WorkoutBlockExercises { get; set; }
}