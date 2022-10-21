using System;
using System.IO;
using System.Threading.Tasks;
using FitnessServices.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers;

public sealed class UserController
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public UserController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [FunctionName("Auth")]
    public async Task<IActionResult> Auth(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "auth")]
        HttpRequest req,
        ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<Auth>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        var auth = await _userService.AuthByEmailPassword(data.Email, data.Password);

        if (auth.Item1 == false)
        {
            return new UnauthorizedResult();
        }
        
        return new OkObjectResult(auth.Item2);
    }

    [FunctionName("GetUser")]
    public async Task<IActionResult> GetUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);
        
        if (authed == false)
        {
            return new UnauthorizedResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _userService.GetUserById(userId);

        return new OkObjectResult(user);
    }
    
    [FunctionName("CreateUser")]
    public async Task<IActionResult> CreateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<Auth>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        await _userService.CreateUser(data.Email, data.Password);

        return new OkObjectResult(true);
    }
}