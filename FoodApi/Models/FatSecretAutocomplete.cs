namespace FoodApi.Models;

public class FatSecretAutocomplete
{
    public FatSecretAutocompleteSuggestion Suggestions { get; set; }
}

public class FatSecretAutocompleteSuggestion
{
    public IEnumerable<string> Suggestion { get; set; }
}