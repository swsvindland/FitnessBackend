namespace FoodApi.Models;

public sealed class FatSecretBarcode
{
    public FatSecretId FoodId { get; set; }
}

public sealed class FatSecretId
{
    public string Value { get; set; }
}