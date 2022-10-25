namespace FoodApi.Models;

public sealed class EdamamParser
{
    public string Text { get; set; }
    public object Parsed { get; set; }
    public IEnumerable<EdamamFoodHint> Hints { get; set; }
}

public sealed class EdamamFoodHint
{
    public EdamamFood Food { get; set; }
    public IEnumerable<EdamamFoodMeasures> Measures { get; set; }
}

public sealed class EdamamFoodMeasures
{
    public string Uri { get; set; }
    public string Label { get; set; }
    public double Weight { get; set; }
}

public sealed class EdamamFood
{
    public string FoodId { get; set; }
    public string Label { get; set; }
    public string KnownAs { get; set; }
    public Dictionary<string, float> Nutrients { get; set; }
    public string Category { get; set; }
    public string CategoryLabel { get; set; }
    public string Image { get; set; }
}
