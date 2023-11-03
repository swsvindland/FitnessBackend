using System;
using System.IO;
using System.Threading.Tasks;
using FitnessRepository.Models;
using FitnessServices.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers;

public sealed class FoodController
{
    private readonly IFoodService _foodService;
    private readonly IAuthService _authService;
    private readonly ILogger<FoodController> _logger;

    public FoodController(IFoodService foodService, IAuthService authService, ILogger<FoodController> logger)
    {
        _foodService = foodService;
        _authService = authService;
        _logger = logger;
    }

    [Function("GetMacros")]
    public async Task<IActionResult> GetMacros(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _foodService.GetUserMacros(userId);

        return new OkObjectResult(user);
    }

    [Function("AddCustomMacros")]
    public async Task<IActionResult> AddCustomMacros(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

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


    [Function("AutocompleteFood")]
    public async Task<IActionResult> AutocompleteFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var query = req.Query["query"];
        var oldToken = req.Query["oldToken"];

        var user = await _foodService.AutocompleteFood(query, oldToken);

        return new OkObjectResult(user);
    }

    [Function("GetRecentUserFoods")]
    public async Task<IActionResult> GetRecentUserFoods(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var user = await _foodService.GetRecentUserFoods(userId, date);

        return new OkObjectResult(user);
    }

    [Function("SearchFood")]
    public async Task<IActionResult> SearchFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var query = req.Query["query"];
        var page = int.Parse(req.Query["page"]);
        var oldToken = req.Query["oldToken"];

        var foods = await _foodService.SearchFood(query, page, oldToken);

        return new OkObjectResult(foods);
    }

    [Function("GetFoodV2")]
    public async Task<IActionResult> GetFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var foodId = long.Parse(req.Query["foodId"]);
        var oldToken = req.Query["oldToken"];

        var foods = await _foodService.GetFoodById(foodId, oldToken);

        return new OkObjectResult(foods);
    }

    [Function("GetUserFoodV2")]
    public async Task<IActionResult> GetUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userFoodId = long.Parse(req.Query["userFoodId"]);

        var foods = await _foodService.GetUserFoodV2(userFoodId);

        return new OkObjectResult(foods);
    }

    [Function("GetAllUserFoodV2")]
    public async Task<IActionResult> GetAllUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var foods = await _foodService.GetAllUserFoodsV2ByDate(userId, date);

        return new OkObjectResult(foods);
    }

    [Function("AddUserFoodV2")]
    public async Task<IActionResult> AddUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

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

    [Function("UpdateUserFoodV2")]
    public async Task<IActionResult> UpdateUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserFoodV2>(requestBody);

        await _foodService.UpdateUserFoodV2(data);

        return new OkObjectResult(true);
    }

    [Function("QuickAddUserFoodV2")]
    public async Task<IActionResult> QuickAddUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var foodId = long.Parse(req.Query["foodId"]);
        var date = DateTime.Parse(req.Query["date"]);
        var oldToken = req.Query["oldToken"];

        var amount = await _foodService.QuickAddUserFoodV2(userId, foodId, date, oldToken);

        return new OkObjectResult(amount);
    }

    [Function("QuickRemoveUserFoodV2")]
    public async Task<IActionResult> QuickRemoveUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var foodId = long.Parse(req.Query["foodId"]);
        var date = DateTime.Parse(req.Query["date"]);
        var oldToken = req.Query["oldToken"];

        var amount = await _foodService.QuickRemoveUserFoodV2(userId, foodId, date, oldToken);

        return new OkObjectResult(amount);
    }

    [Function("DeleteUserFoodV2")]
    public async Task<IActionResult> DeleteUserFoodV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userFoodId = long.Parse(req.Query["userFoodId"]);

        await _foodService.DeleteUserFoodV2(userFoodId);

        return new OkObjectResult(true);
    }

    [Function("GetCurrentUserMacrosV2")]
    public async Task<IActionResult> GetCurrentUserMacrosV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var user = await _foodService.GetUserCurrentMacosV2(userId, date);

        return new OkObjectResult(user);
    }

    [Function("SearchFoodByBarcode")]
    public async Task<IActionResult> SearchFoodByBarcode(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var barcode = req.Query["barcode"];
        var oldToken = req.Query["oldToken"];

        var user = await _foodService.GetFoodByBarcode(barcode, oldToken);

        return new OkObjectResult(user);
    }

    [Function("FoodApiAuth")]
    public async Task<IActionResult> FoodApiAuth(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var token = await _foodService.AuthFatSecretApi();

        return new OkObjectResult(token);
    }

    [Function("MaliciousComplianceHTTP")]
    public async Task MaliciousComplianceHttp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        await _foodService.RefreshCashedFoodDb(_logger);
    }

    // Will run every day at 4:30am, refreshing food db, in compliance with https://platform.fatsecret.com/api/Default.aspx?screen=tou 1.5
    // [Function("MaliciousCompliance")]
    // public async Task MaliciousCompliance([TimerTrigger("0 30 4 * * *")]TimerInfo myTimer)
    // {
    //     if (myTimer.IsPastDue)
    //     {
    //         log.LogInformation("Timer is running late!");
    //     }
    //     log.LogInformation("C# Timer trigger function executed at: {Now}", DateTime.Now);
    //     
    //     await _foodService.RefreshCashedFoodDb(log);
    // }
}