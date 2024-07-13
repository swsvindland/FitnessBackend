using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessRepository.Models;

public sealed class Supplements
{
    public long Id { get; set; }
    public DateTime Created { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    [Column(TypeName = "varchar(255)")]
    public SupplementIcon? Icon { get; set; }
    
}

public enum SupplementIcon
{
    Unknown,
    SmallScoop,
    LargeScoop,
    Capsule,
    Tablet,
    Liquid,
    Injection,
}