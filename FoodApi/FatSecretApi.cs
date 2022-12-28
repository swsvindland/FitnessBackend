using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;
using FoodApi.Models;

namespace FoodApi;

public class FatSecretApi : IFatSecretApi
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public FatSecretApi(HttpClient client)
    {
        _client = client;
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
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var values = new Dictionary<string, string>
        {
            {"scope", "basic"},
            {"grant_type", "client_credentials"},
            {"format", "json"}
        };
        var content = new FormUrlEncodedContent(values);
        var response = await client.PostAsync("https://oauth.fatsecret.com/connect/token", content);

        var json = await response.Content.ReadFromJsonAsync<FatSecretAuth>(_jsonSerializerOptions);
        return json?.AccessToken ?? string.Empty;
    }

    public async Task<IEnumerable<FatSecretSearchItem>> SearchFoods(string query, int pageNumber)
    {
        try
        {
            var token = await AuthFatSecretApi();
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response =
                await _client.PostAsync(
                    $"https://platform.fatsecret.com/rest/server.api?method=foods.search&search_expression={query}&format=json&max_results={50}&page_number={pageNumber}",
                    null);
            var result = await response.Content.ReadFromJsonAsync<FatSecretSearch>(_jsonSerializerOptions);
            return result.foods.food;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<FatSecretItem> GetFood(long foodId)
    {
        try
        {
            var token = await AuthFatSecretApi();
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response =
                    await _client.PostAsync(
                        $"https://platform.fatsecret.com/rest/server.api?method=food.get.v2&food_id={foodId}&format=json",
                        null);

                var result = await response.Content.ReadFromJsonAsync<FatSecretGet>(_jsonSerializerOptions);
                return result.food;
            }
            catch (Exception e)
            {
                var response =
                    await _client.PostAsync(
                        $"https://platform.fatsecret.com/rest/server.api?method=food.get.v2&food_id={foodId}&format=json",
                        null);

                var result =
                    await response.Content.ReadFromJsonAsync<FatSecretGetSingleServing>(_jsonSerializerOptions);

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
            return null;
        }
    }
}