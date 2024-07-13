using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Models;

public sealed class Food
{ 
    public long Id { get; set; }
    public long ExternalId { get; set; }
    public string? Brand { get; set; }
    public string Name { get; set; }
    public string FoodType { get; set; }
    public DateTime Created { get; set; }
    [ConcurrencyCheck]
    public DateTime? Updated { get; set; }
    public IEnumerable<FoodServings> Servings { get; set; }
}