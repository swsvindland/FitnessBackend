using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessRepository;

[Table("supplements")]
public class Supplements
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public string? Url { get; set; }
    public int? Commission { get; set; }
}