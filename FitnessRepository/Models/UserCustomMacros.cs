namespace FitnessRepository.Models;

public sealed class UserCustomMacros
{
    public long? Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public float Calories { get; set; }
    public float Protein { get; set; }
    public float Fat { get; set; }
    public float Carbs { get; set; }
    public float Fiber { get; set; }
    public float? CaloriesHigh { get; set; }
    public float? ProteinHigh { get; set; }
    public float? FatHigh { get; set; }
    public float? CarbsHigh { get; set; }
    public float? FiberHigh { get; set; }
}