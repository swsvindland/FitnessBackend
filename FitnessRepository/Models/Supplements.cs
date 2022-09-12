namespace FitnessRepository.Models;

public sealed class Supplements
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string? Url { get; set; }
    public int? Commission { get; set; }
}