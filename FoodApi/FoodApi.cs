using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FoodApi.Models;

namespace FoodApi;

public sealed class FoodApi : IFoodApi
{
    private readonly string _appId;
    private readonly string _appKey;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public FoodApi(HttpClient client)
    {
        _client = client;
        _appId = "507a3f35";
        _appKey = "7f691d4ec6672e60f3864543f6a00efe";
        _clientId = "91ab797e9a424b0aa7f0eb6a6861bb59";
        _clientSecret = "ea9adc557e6d41378c16615285e1c4d0";
        
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };
    }

    public async Task<string> AuthFatSecretApi()
    {
        var client = new HttpClient();
        var byteArray = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var values = new Dictionary<string, string>
        {
            { "scope", "basic" },
            { "grant_type", "client_credentials" }
        };
        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("https://oauth.fatsecret.com/connect/token", content);

        var json = await response.Content.ReadFromJsonAsync<FatSecretAuth>(_jsonSerializerOptions);
        return json?.AccessToken ?? string.Empty;
    }

    public async Task<FatSecretSearch?> ParseFood(string foodQuery, string? barcode)
    {
        try
        {
            var token = await AuthFatSecretApi();
            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            
            var content = new Search()
            {
                Method = "foods.search",
                SearchExpression = foodQuery,
            };

            var json = JsonSerializer.Serialize(content, _jsonSerializerOptions);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response =
                await _client.PostAsync(
                    $"https://platform.fatsecret.com/rest/server.api", data);
            var result = await response.Content.ReadFromJsonAsync<FatSecretSearch>(_jsonSerializerOptions);
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<EdamamNutrients?> Nutrients(string foodId, float servingSizeInGrams)
    {
        try
        {
            var content = new Content()
            {
                Ingredients = new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Quantity = servingSizeInGrams,
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

public sealed class Search
{
    public string Method { get; set; }
    public string SearchExpression { get; set; }
    public int MaxResults { get; set; } = 100;
    public string Format { get; set; }
}

public sealed class Content
{
    public IEnumerable<Ingredient> Ingredients { get; set; }
}

public sealed class Ingredient
{
    public float Quantity { get; set; }
    public string MeasureURI { get; set; }
    public string FoodId { get; set; }
}