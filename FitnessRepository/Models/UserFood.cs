using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessRepository.Models;

public sealed class UserFood
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long? FoodId { get; set; }
    public string? EdamamFoodId { get; set; }
    public Food? Food { get; set; }
    public DateTime Created { get; set; }
    public float Amount { get; set; }
    [Column(TypeName = "varchar(255)")]
    public Units Unit { get; set; }
}