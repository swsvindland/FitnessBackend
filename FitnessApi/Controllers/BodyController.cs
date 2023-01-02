using System;
using System.IO;
using System.Threading.Tasks;
using FitnessRepository.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers;

public sealed class BodyController
{
    private readonly IBodyService _bodyService;
    private readonly IAuthService _authService;

    public BodyController(IBodyService bodyService, IAuthService authService)
    {
        _bodyService = bodyService;
        _authService = authService;
    }

    [FunctionName("GetUserWeights")]
    public async Task<IActionResult> GetUserWeights(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);
        
        if (authed == false)
        {
            return new UnauthorizedResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GetAllUserWeights(userId);

        return new OkObjectResult(user);
    }

    [FunctionName("AddUserWeight")]
    public async Task<IActionResult> AddUserWeight(
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

        var data = JsonConvert.DeserializeObject<UserWeight>(requestBody);

        await _bodyService.AddUserWeight(data);

        return new OkObjectResult(true);
    }

    [FunctionName("GetUserBodies")]
    public async Task<IActionResult> GetUserBodies(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);
        
        if (authed == false)
        {
            return new UnauthorizedResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GetAllUserBodies(userId);

        return new OkObjectResult(user);
    }

    [FunctionName("AddUserBody")]
    public async Task<IActionResult> AddUserBody(
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

        var data = JsonConvert.DeserializeObject<UserBody>(requestBody);

        await _bodyService.AddUserBody(data);

        return new OkObjectResult(true);
    }

    [FunctionName("GetUserBloodPressures")]
    public async Task<IActionResult> GetUserBloodPressures(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);
        
        if (authed == false)
        {
            return new UnauthorizedResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GetAllUserBloodPressures(userId);

        return new OkObjectResult(user);
    }

    [FunctionName("GetUserBodyFat")]
    public async Task<IActionResult> GetUserBodyFat(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);
        
        if (authed == false)
        {
            return new UnauthorizedResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GenerateBodyFats(userId);

        return new OkObjectResult(user);
    }

    [FunctionName("AddUserBloodPressure")]
    public async Task<IActionResult> AddUserBloodPressure(
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

        var data = JsonConvert.DeserializeObject<UserBloodPressure>(requestBody);

        await _bodyService.AddUserBloodPressure(data);

        return new OkObjectResult(true);
    }

    [FunctionName("AddUserHeight")]
    public async Task<IActionResult> AddUserHeight(
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

        var data = JsonConvert.DeserializeObject<UserHeight>(requestBody);

        await _bodyService.AddUserHeight(data);

        return new OkObjectResult(true);
    }
}