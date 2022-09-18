namespace FoodApi.Models;

public class EdamamNutrients
{
    public float Calories { get; set; }
    public float TotalWeights { get; set; }
    public Dictionary<string, EdamamNutrient> TotalNutrients { get; set; }
}

public class EdamamNutrient
{
    public string Label { get; set; }
    public float Quantity { get; set; }
    public string Unit { get; set; }
}