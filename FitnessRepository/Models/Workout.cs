namespace FitnessRepository.Models;

public sealed class Workout
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public int Version { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Days { get; set; }
    public int Duration { get; set; }
}