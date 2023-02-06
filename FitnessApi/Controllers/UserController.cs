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
    
    [FunctionName("AuthV2")]
    public async Task<IActionResult> AuthV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
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

        var auth = await _userService.AuthByEmailPasswordV2(data.Email, data.Password);

        if (auth == null)
        {
            return new UnauthorizedResult();
        }
        
        return new OkObjectResult(auth);
    }
    
    [FunctionName("SsoAuthV2")]
    public async Task<IActionResult> SsoAuthV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<SsoAuth>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        var auth = await _userService.SsoAuth(data.Email, data.Token);

        if (auth == null)
        {
            return new UnauthorizedResult();
        }
        
        return new OkObjectResult(auth);
    }
    
    [FunctionName("ChangePassword")]
    public async Task<IActionResult> ChangePassword(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<ChangePassword>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);
        
        await _userService.ChangePassword(userId, data.OldPassword, data.NewPassword);
        
        return new OkObjectResult(true);
    }
    
    [FunctionName("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        var email = req.Query["email"];
        
        await _userService.ForgotPassword(email);
        
        return new OkObjectResult(true);
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

        await _userService.UpdateLastLogin(userId);
        await _userService.CheckIfPaidUntilValid(userId);
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
    
    [FunctionName("UpdateUserSex")]
    public async Task<IActionResult> UpdateUserSex(
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

        var data = JsonConvert.DeserializeObject<UpdateSex>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);
        
        await _userService.UpdateUserSex(userId, data.Sex);

        return new OkObjectResult(true);
    }
    
    [FunctionName("UpdateUserPaid")]
    public async Task<IActionResult> UpdateUserPaid(
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

        var data = JsonConvert.DeserializeObject<UpdatePaid>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);
        
        await _userService.UpdatePaid(userId, data.Paid, data.PaidUntil);

        return new OkObjectResult(true);
    }
    
    [FunctionName("UpdateUserUnits")]
    public async Task<IActionResult> UpdateUserUnits(
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

        var data = JsonConvert.DeserializeObject<UpdateUnit>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);
        
        await _userService.UpdateUserUnits(userId, data.Unit);

        return new OkObjectResult(true);
    }
    
    [FunctionName("DeleteUser")]
    public async Task<IActionResult> DeleteUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);
        
        if (authed == false)
        {
            return new UnauthorizedResult();
        }
        
        var userId = Guid.Parse(req.Query["userId"]);

        await _userService.DeleteUser(userId);

        return new OkObjectResult(true);
    }
}