namespace FoodApi.Models;

public sealed class FatSecretSearch
{
    public FatSecretSearchList foods { get; set; }
}

public sealed class FatSecretSearchList
{
    public IEnumerable<FatSecretSearchItem> food { get; set; }
}

public sealed class FatSecretSearchItem
{
    public string FoodId { get; set; }
    public string FoodName { get; set; }
    public string FoodType { get; set; }
    public string FoodUrl { get; set; }
    public string FoodDescription { get; set; }
}