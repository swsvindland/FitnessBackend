namespace FitnessRepository.Models;

public class Exercise
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public int Fatigue { get; set; }
}