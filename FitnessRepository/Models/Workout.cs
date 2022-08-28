namespace FitnessRepository.Models;

public class Workout
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public int Version { get; set; }
    public string Name { get; set; }
    public float? Cost { get; set; }
}