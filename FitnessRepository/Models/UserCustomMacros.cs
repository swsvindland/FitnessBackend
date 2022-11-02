namespace FitnessRepository.Models;

public class UserCustomMacros
{
    public long? Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Created { get; set; }
    public float Calories { get; set; }
    public float Protein { get; set; }
    public float Fat { get; set; }
    public float Carbs { get; set; }
    public float Fiber { get; set; }
    public float Alcohol { get; set; }
    public float Water { get; set; }
}