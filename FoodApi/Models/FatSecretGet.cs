namespace FoodApi.Models;

public sealed class FatSecretGet
{
    public FatSecretItem food { get; set; }
}

public sealed class FatSecretItem
{
    public string FoodId { get; set; }
    public string? BrandName { get; set; }
    public string FoodName { get; set; }
    public string FoodType { get; set; }
    public string FoodUrl { get; set; }
    public FatSecretServings Servings { get; set; }
}

public sealed class FatSecretServings
{
    public IEnumerable<FatSecretServing> Serving { get; set; }
}


public sealed class FatSecretGetSingleServing
{
    public FatSecretItemSingleServing food { get; set; }
}

public sealed class FatSecretItemSingleServing
{
    public string FoodId { get; set; }
    public string? BrandName { get; set; }
    public string FoodName { get; set; }
    public string FoodType { get; set; }
    public string FoodUrl { get; set; }
    public FatSecretSingleServing Servings { get; set; }
}

public sealed class FatSecretSingleServing
{
    public FatSecretServing Serving { get; set; }
}

public sealed class FatSecretServing
{
    public string? AddedSugar { get; set; }
    public string? Calcium { get; set; }
    public string? Calories { get; set; }
    public string? Carbohydrate { get; set; }
    public string? Cholesterol { get; set; }
    public string? Fat { get; set; }
    public string? Fiber { get; set; }
    public string? Iron { get; set; }
    public string? MeasurementDescription { get; set; }
    public string? MetricServingAmount { get; set; }
    public string? MetricServingUnit { get; set; }
    public string? MonounsaturatedFat { get; set; }
    public string? NumberOfUnits { get; set; }
    public string? PolyunsaturatedFat { get; set; }
    public string? Potassium { get; set; }
    public string? Protein { get; set; }
    public string? SaturatedFat { get; set; }
    public string? ServingDescription { get; set; }
    public string? ServingId { get; set; }
    public string? ServingUrl { get; set; }
    public string? Sodium { get; set; }
    public string? Sugar { get; set; }
    public string? TransFat { get; set; }
    public string? VitaminA { get; set; }
    public string? VitaminC { get; set; }
    public string? VitaminD { get; set; }
}