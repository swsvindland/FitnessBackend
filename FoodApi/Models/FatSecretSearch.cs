namespace FoodApi.Models;

public class FatSecretSearch
{
    public FatSecretSearchList foods { get; set; }
}

public class FatSecretSearchList
{
    public IEnumerable<FatSecretSearchItem> food { get; set; }
}

public class FatSecretSearchItem
{
    public string FoodId { get; set; }
    public string FoodName { get; set; }
    public string FoodType { get; set; }
    public string FoodUrl { get; set; }
    public string FoodDescription { get; set; }
}