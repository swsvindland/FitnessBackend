namespace FitnessRepository.Models;

public sealed class FoodV2
{
    public long Id { get; set; }
    public string? Brand { get; set; }
    public string Name { get; set; }
    public string FoodType { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public IEnumerable<FoodV2Servings> Servings { get; set; }
}