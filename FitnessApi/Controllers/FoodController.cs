using System;
using System.IO;
using System.Threading.Tasks;
using FitnessRepository.Models;
using FitnessServices.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers;

public sealed class FoodController
{
    private readonly IFoodService _foodService;

    public FoodController(IFoodService foodService)
    {
        _foodService = foodService;
    }

    [FunctionName("GetMacros")]
    public async Task<IActionResult> GetMacros(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _foodService.GetUserMacros(userId);

        return new OkObjectResult(user);
    }
    
    [FunctionName("AddCustomMacros")]
    public async Task<IActionResult> AddCustomMacros(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<Macros>(requestBody);
        
        var userId = Guid.Parse(req.Query["userId"]);

        await _foodService.AddUserCustomMacros(userId, data);

        return new OkObjectResult(true);
    }


    [FunctionName("AutocompleteFood")]
    public async Task<IActionResult> AutocompleteFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var query = req.Query["query"];

        var user = await _foodService.AutocompleteFood(query);

        return new OkObjectResult(user);
    }

    [FunctionName("GetRecentUserFoods")]
    public async Task<IActionResult> GetRecentUserFoods(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var user = await _foodService.GetRecentUserFoods(userId, date);

        return new OkObjectResult(user);
    }

    [FunctionName("SearchFood")]
    public async Task<IActionResult> SearchFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var query = req.Query["query"];
        var page = int.Parse(req.Query["page"]);

        var foods = await _foodService.SearchFood(query, page);

        return new OkObjectResult(foods);
    }
    
    [FunctionName("GetFoodV2")]
    public async Task<IActionResult> GetFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var foodId = long.Parse(req.Query["foodId"]);

        var foods = await _foodService.GetFoodById(foodId);

        return new OkObjectResult(foods);
    }
    
    [FunctionName("GetUserFoodV2")]
    public async Task<IActionResult> GetUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userFoodId = long.Parse(req.Query["userFoodId"]);

        var foods = await _foodService.GetUserFoodV2(userFoodId);

        return new OkObjectResult(foods);
    }
    
    [FunctionName("GetAllUserFoodV2")]
    public async Task<IActionResult> GetAllUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var foods = await _foodService.GetAllUserFoodsV2ByDate(userId, date);

        return new OkObjectResult(foods);
    }
    
    [FunctionName("AddUserFoodV2")]
    public async Task<IActionResult> AddUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserFoodV2>(requestBody);
        
        var date = DateTime.Parse(req.Query["date"]);

        var id = await _foodService.AddUserFoodV2(data, date);

        return new OkObjectResult(id);
    }
    
    [FunctionName("UpdateUserFoodV2")]
    public async Task<IActionResult> UpdateUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserFoodV2>(requestBody);

        await _foodService.UpdateUserFoodV2(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("QuickAddUserFoodV2")]
    public async Task<IActionResult> QuickAddUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var foodId = long.Parse(req.Query["foodId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var amount = await _foodService.QuickAddUserFoodV2(userId, foodId, date);

        return new OkObjectResult(amount);
    }
    
    [FunctionName("QuickRemoveUserFoodV2")]
    public async Task<IActionResult> QuickRemoveUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var foodId = long.Parse(req.Query["foodId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var amount = await _foodService.QuickRemoveUserFoodV2(userId, foodId, date);

        return new OkObjectResult(amount);
    }
    
    [FunctionName("DeleteUserFoodV2")]
    public async Task<IActionResult> DeleteUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userFoodId = long.Parse(req.Query["userFoodId"]);
        
        await _foodService.DeleteUserFoodV2(userFoodId);

        return new OkObjectResult(true);
    }
    
    [FunctionName("GetCurrentUserMacrosV2")]
    public async Task<IActionResult> GetCurrentUserMacrosV2(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var user = await _foodService.GetUserCurrentMacosV2(userId, date);

        return new OkObjectResult(user);
    }
    
    [FunctionName("SearchFoodByBarcode")]
    public async Task<IActionResult> SearchFoodByBarcode(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var barcode = req.Query["barcode"];

        var user = await _foodService.GetFoodByBarcode(barcode);

        return new OkObjectResult(user);
    }

    [FunctionName("MaliciousComplianceHTTP")]
    public async Task MaliciousComplianceHttp([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        await _foodService.RefreshCashedFoodDb(log);
    }
    
    // Will run every day at 4:30am, refreshing food db, in compliance with https://platform.fatsecret.com/api/Default.aspx?screen=tou 1.5
    [FunctionName("MaliciousCompliance")]
    public async Task MaliciousCompliance([TimerTrigger("0 30 4 * * *")]TimerInfo myTimer, ILogger log)
    {
        if (myTimer.IsPastDue)
        {
            log.LogInformation("Timer is running late!");
        }
        log.LogInformation("C# Timer trigger function executed at: {Now}", DateTime.Now);
        
        await _foodService.RefreshCashedFoodDb(log);
    }
}