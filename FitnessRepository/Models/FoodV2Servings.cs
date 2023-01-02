namespace FitnessRepository.Models;

public sealed class FoodV2Servings
{
    public long Id { get; set; }
    public long FoodV2Id { get; set; }
    public float? AddedSugar { get; set; }
    public float? Calcium { get; set; }
    public float? Calories { get; set; }
    public float? Carbohydrate { get; set; }
    public float? Cholesterol { get; set; }
    public float? Fat { get; set; }
    public float? Fiber { get; set; }
    public float? Iron { get; set; }
    public string? MeasurementDescription { get; set; }
    public float? MetricServingAmount { get; set; }
    public string? MetricServingUnit { get; set; }
    public float? MonounsaturatedFat { get; set; }
    public float? NumberOfUnits { get; set; }
    public float? PolyunsaturatedFat { get; set; }
    public float? Potassium { get; set; }
    public float? Protein { get; set; }
    public float? SaturatedFat { get; set; }
    public string? ServingDescription { get; set; }
    public float? Sodium { get; set; }
    public float? Sugar { get; set; }
    public float? TransFat { get; set; }
    public float? VitaminA { get; set; }
    public float? VitaminC { get; set; }
    public float? VitaminD { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}