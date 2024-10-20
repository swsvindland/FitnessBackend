using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FoodApi.Models;

namespace FoodApi;

public sealed class FatSecretApi : IFatSecretApi
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public FatSecretApi(HttpClient client)
    {
        _client = client;
        _clientId = Environment.GetEnvironmentVariable("FAT_SECRET_KEY") ?? "";
        _clientSecret = Environment.GetEnvironmentVariable("FAT_SECRET_SECRET") ?? "";

        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };
    }

    public async Task<FatSecretAuth?> AuthFatSecretApi()
    {
        var client = new HttpClient();
        var byteArray = Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var values = new Dictionary<string, string>
        {
            {"scope", "basic premier barcode"},
            {"grant_type", "client_credentials"},
            {"format", "json"}
        };
        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("https://oauth.fatsecret.com/connect/token", content);

        return await response.Content.ReadFromJsonAsync<FatSecretAuth>(_jsonSerializerOptions);
    }

    public async Task<IEnumerable<FatSecretSearchItem>?> SearchFoods(string query, int pageNumber, string? oldToken)
    {
        try
        {
            var token = !string.IsNullOrEmpty(oldToken) ? oldToken : (await AuthFatSecretApi())?.AccessToken;
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response =
                await _client.PostAsync(
                    $"https://platform.fatsecret.com/rest/server.api?method=foods.search&search_expression={query}&format=json&max_results={50}&page_number={pageNumber}",
                    null);
            var result = await response.Content.ReadFromJsonAsync<FatSecretSearch>(_jsonSerializerOptions);
            return result?.foods.food;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error while searching foods");
        }
    }

    public async Task<FatSecretItem?> GetFood(long foodId, string? oldToken)
    {
        try
        {
            var token = !string.IsNullOrEmpty(oldToken) ? oldToken : (await AuthFatSecretApi())?.AccessToken;
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response =
                    await _client.PostAsync(
                        $"https://platform.fatsecret.com/rest/server.api?method=food.get.v2&food_id={foodId}&format=json",
                        null);

                var result = await response.Content.ReadFromJsonAsync<FatSecretGet>(_jsonSerializerOptions);
                return result?.food;
            }
            catch (Exception e)
            {
                var response =
                    await _client.PostAsync(
                        $"https://platform.fatsecret.com/rest/server.api?method=food.get.v2&food_id={foodId}&format=json",
                        null);

                var result =
                    await response.Content.ReadFromJsonAsync<FatSecretGetSingleServing>(_jsonSerializerOptions);

                if (result == null)
                {
                    return null;
                }

                var item = new FatSecretItem()
                {
                    BrandName = result.food.BrandName,
                    FoodId = result.food.FoodId,
                    FoodName = result.food.FoodName,
                    FoodType = result.food.FoodType,
                    FoodUrl = result.food.FoodUrl,
                    Servings = new FatSecretServings()
                    {
                        Serving = new List<FatSecretServing>()
                        {
                            result.food.Servings.Serving
                        }
                    }
                };

                return item;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error while searching foods");
        }
    }
    
    public async Task<long?> GetIdFromBarcode(string barcode, string? oldToken)
    {
        try
        {
            var token = !string.IsNullOrEmpty(oldToken) ? oldToken : (await AuthFatSecretApi())?.AccessToken;
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response =
                await _client.PostAsync(
                    $"https://platform.fatsecret.com/rest/server.api?method=food.find_id_for_barcode&barcode={barcode}&format=json",
                    null);
            var result = await response.Content.ReadFromJsonAsync<FatSecretBarcode>(_jsonSerializerOptions);
            return long.TryParse(result?.FoodId.Value, out var id) ? id : null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Error while searching foods");
        }
    }
    
    public async Task<IEnumerable<string>?> Autocomplete(string query, string? oldToken)
    {
        try
        {
            var token = !string.IsNullOrEmpty(oldToken) ? oldToken : (await AuthFatSecretApi())?.AccessToken;
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response =
                await _client.PostAsync(
                    $"https://platform.fatsecret.com/rest/server.api?method=foods.autocomplete&expression={query}&format=json",
                    null);
            var result = await response.Content.ReadFromJsonAsync<FatSecretAutocomplete>(_jsonSerializerOptions);
            return result?.Suggestions.Suggestion;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}