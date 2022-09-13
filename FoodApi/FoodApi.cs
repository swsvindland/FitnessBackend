using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FoodApi;

public class FoodApi: IFoodApi
{
    private readonly string _appId;
    private readonly string _appKey;
    
    public FoodApi()
    {
        _appId = "507a3f35";
        _appKey = "7f691d4ec6672e60f3864543f6a00efe";
    }
    
    public async Task<IEnumerable<string>?> AutocompleteFood(string query)
    {
        try
        {
            var client = new HttpClient();
            var url = $"https://api.edamam.com/auto-complete?app_id={_appId}&app_key={_appKey}&q={query}";
            var response = await client.GetAsync(url);
            return await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}