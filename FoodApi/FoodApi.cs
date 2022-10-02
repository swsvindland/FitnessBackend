using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FoodApi.Models;

namespace FoodApi;

public sealed class FoodApi: IFoodApi
{
    private readonly string _appId;
    private readonly string _appKey;
    private readonly HttpClient _client;
    
    public FoodApi(HttpClient client)
    {
        _client = client;
        _appId = "507a3f35";
        _appKey = "7f691d4ec6672e60f3864543f6a00efe";
    }
    
    public async Task<IEnumerable<EdamamFood>?> ParseFood(string foodQuery)
    {
        try
        {
            var response =
                await _client.GetAsync(
                    $"https://api.edamam.com/api/food-database/v2/parser?ingr={foodQuery}&app_id={_appId}&app_key={_appKey}");
            var result = await response.Content.ReadFromJsonAsync<EdamamParser>();
            return result?.Hints.Select(e => e.Food);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
    
    public async Task<EdamamNutrients?> Nutrients(string foodId)
    {
        try
        {
            var content = new Content()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Quantity = 100,
                        MeasureURI = "Gram",
                        FoodId = foodId
                    }
                }
            };

            var json = JsonSerializer.Serialize(content, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            
            var data = new StringContent(json, Encoding.UTF8, "application/json");
                
            var response =
                await _client.PostAsync(
                    $"https://api.edamam.com/api/food-database/v2/nutrients?app_id={_appId}&app_key={_appKey}", data);
            var result = await response.Content.ReadFromJsonAsync<EdamamNutrients>();
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<IEnumerable<string>?> AutocompleteFood(string query)
    {
        try
        {
            var url = $"https://api.edamam.com/auto-complete?app_id={_appId}&app_key={_appKey}&q={query}";
            var response = await _client.GetAsync(url);
            return await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}

public sealed class Content
{
    public IEnumerable<Ingredient> Ingredients { get; set; }
}

public sealed class Ingredient
{
    public int Quantity { get; set; }
    public string MeasureURI { get; set; }
    public string FoodId { get; set; }

}