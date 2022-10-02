using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessRepository.Models;

public class Food
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public int ServingSize { get; set; }
    [Column(TypeName = "varchar(255)")]
    public Units ServingSizeUnit { get; set; }
    public float Calories { get; set; }
    public float? TotalFat { get; set; }
    public float? SaturatedFat { get; set; }
    public float? TransFat { get; set; }
    public float? MonounsaturatedFat { get; set; }
    public float? PolyunsaturatedFat { get; set; }
    public float? Carbohydrates { get; set; }
    public float? Fiber { get; set; }
    public float? Sugar { get; set; }
    public float? Protein { get; set; }
    public float? Cholesterol { get; set; }
    public float? Sodium { get; set; }
    public float? Potassium { get; set; }
    public float? Calcium { get; set; }
    public float? Iron { get; set; }
    public float? Magnesium { get; set; }
    public float? Zinc { get; set; }
    public float? Phosphorus { get; set; }
    public float? VitaminA { get; set; }
    public float? VitaminC { get; set; }
    public float? VitaminD { get; set; }
    public float? VitaminE { get; set; }
    public float? VitaminK { get; set; }
    public float? Thiamin { get; set; }
    public float? Riboflavin { get; set; }
    public float? Niacin { get; set; }
    public float? VitaminB6 { get; set; }
    public float? Folate { get; set; }
    public float? VitaminB12 { get; set; }
    public float? Water { get; set; }
    public float? Alcohol { get; set; }
    public float? Caffeine { get; set; }
}