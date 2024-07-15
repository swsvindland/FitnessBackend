using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FitnessRepository.Models;

public sealed class Food
{ 
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }
    public string? Brand { get; set; }
    public string Name { get; set; }
    public string FoodType { get; set; }
    public DateTime Created { get; set; }
    [ConcurrencyCheck]
    public DateTime? Updated { get; set; }
    public IEnumerable<FoodServings> Servings { get; set; }
}