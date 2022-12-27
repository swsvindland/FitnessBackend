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
    private readonly IAuthService _authService;

    public FoodController(IFoodService foodService, IAuthService authService)
    {
        _foodService = foodService;
        _authService = authService;
    }

    [FunctionName("GetMacros")]
    public async Task<IActionResult> GetMacros(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _foodService.GetUserMacros(userId);

        return new OkObjectResult(user);
    }
    
    [FunctionName("AddCustomMacros")]
    public async Task<IActionResult> AddCustomMacros(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

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


    [FunctionName("AutocompleteFood")]
    public async Task<IActionResult> AutocompleteFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var query = req.Query["query"];

        var user = await _foodService.AutocompleteFood(query);

        return new OkObjectResult(user);
    }

    [FunctionName("ParseFood")]
    public async Task<IActionResult> ParseFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var query = req.Query["query"];
        var barcode = req.Query["barcode"];

        var user = await _foodService.ParseFood(query, barcode);

        return new OkObjectResult(user);
    }

    [FunctionName("GetFoodDetails")]
    public async Task<IActionResult> GetFoodDetails(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var foodId = req.Query["foodId"];
        var servingSize = float.Parse(req.Query["servingSize"]);

        var user = await _foodService.GetFoodDetails(foodId, servingSize);

        return new OkObjectResult(user);
    }

    [FunctionName("GetUserFoods")]
    public async Task<IActionResult> GetUserFoods(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var user = await _foodService.GetUserFoods(userId, date);

        return new OkObjectResult(user);
    }

    [FunctionName("GetUserFoodsForGrid")]
    public async Task<IActionResult> GetUserFoodsForGrid(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var user = await _foodService.GetUserFoodsForGrid(userId, date);

        return new OkObjectResult(user);
    }
    
    [FunctionName("GetUserFood")]
    public async Task<IActionResult> GetUserFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);
        var foodId = long.Parse(req.Query["foodId"]);

        var user = await _foodService.GetUserFood(userId, date, foodId);

        return new OkObjectResult(user);
    }
    
    [FunctionName("UpdateUserFood")]
    public async Task<IActionResult> UpdateUserFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }
        
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserFood>(requestBody);

        await _foodService.UpdateUserFood(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("DeleteUserFood")]
    public async Task<IActionResult> DeleteUserFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userFoodId = long.Parse(req.Query["userFoodId"]);

        await _foodService.DeleteUserFood(userFoodId);

        return new OkObjectResult(true);
    }
    
    [FunctionName("GetFood")]
    public async Task<IActionResult> GetFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var foodId = long.Parse(req.Query["foodId"]);

        var user = await _foodService.GetFood(foodId);

        return new OkObjectResult(user);
    }

    [FunctionName("GetCurrentUserMacros")]
    public async Task<IActionResult> GetCurrentUserMacros(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var user = await _foodService.GetUserCurrentMacos(userId, date);

        return new OkObjectResult(user);
    }


    [FunctionName("AddUserFood")]
    public async Task<IActionResult> AddUserFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserFood>(requestBody);

        await _foodService.AddUserFood(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("SearchFood")]
    public async Task<IActionResult> SearchFood(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        // var authed = await _authService.CheckAuth(req);
        //
        // if (authed == false)
        // {
        //     return new UnauthorizedResult();
        // }

        var query = req.Query["query"];
        var page = int.Parse(req.Query["page"]);

        var foods = await _foodService.SearchFood(query, page);

        return new OkObjectResult(foods);
    }
}