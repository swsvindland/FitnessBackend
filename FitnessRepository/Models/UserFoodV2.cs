namespace FitnessRepository.Models;

public sealed class UserFoodV2
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public long FoodV2Id { get; set; }
    public FoodV2? FoodV2 { get; set; }
    public long ServingId { get; set; }
    public FoodV2Servings? Serving { get; set; }
    public float ServingAmount { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}