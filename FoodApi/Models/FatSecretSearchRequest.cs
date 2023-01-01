namespace FoodApi.Models;

public class FatSecretSearchRequest
{
    public string Method { get; set; }
    public string SearchExpression { get; set; }
    public int MaxResults { get; set; } = 50;
    public string Format { get; set; } = "json";
}