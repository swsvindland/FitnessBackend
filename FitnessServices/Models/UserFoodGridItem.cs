using FitnessRepository.Models;

namespace FitnessServices.Models;

public class UserFoodGridItem
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long? FoodId { get; set; }
    public string? EdamamFoodId { get; set; }
    public Food? Food { get; set; }
    public DateTime Created { get; set; }
    public float Amount { get; set; }
    public float Servings {get; set;}
}