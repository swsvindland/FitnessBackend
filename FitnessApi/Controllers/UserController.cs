using System;
using System.IO;
using System.Threading.Tasks;
using FitnessServices.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers;

public sealed class UserController
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, IAuthService authService, ILogger<UserController> logger)
    {
        _userService = userService;
        _authService = authService;
        _logger = logger;
    }

    [Function("AuthV2")]
    public async Task<IActionResult> AuthV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
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

    [Function("SsoAuthV2")]
    public async Task<IActionResult> SsoAuthV2(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
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

    [Function("ChangePassword")]
    public async Task<IActionResult> ChangePassword(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
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

    [Function("ForgotPassword")]
    public async Task<IActionResult> ForgotPassword(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var email = req.Query["email"];

        await _userService.ForgotPassword(email);

        return new OkObjectResult(true);
    }

    [Function("GetUser")]
    public async Task<IActionResult> GetUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

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

    [Function("CreateUser")]
    public async Task<IActionResult> CreateUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
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

    [Function("UpdateUserSex")]
    public async Task<IActionResult> UpdateUserSex(
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

        var data = JsonConvert.DeserializeObject<UpdateSex>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        await _userService.UpdateUserSex(userId, data.Sex);

        return new OkObjectResult(true);
    }

    [Function("UpdateUserPaid")]
    public async Task<IActionResult> UpdateUserPaid(
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

        var data = JsonConvert.DeserializeObject<UpdatePaid>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        await _userService.UpdatePaid(userId, data.Paid, data.PaidUntil);

        return new OkObjectResult(true);
    }

    [Function("UpdateUserUnits")]
    public async Task<IActionResult> UpdateUserUnits(
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

        var data = JsonConvert.DeserializeObject<UpdateUnit>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        await _userService.UpdateUserUnits(userId, data.Unit);

        return new OkObjectResult(true);
    }

    [Function("DeleteUser")]
    public async Task<IActionResult> DeleteUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        await _userService.DeleteUser(userId);

        return new OkObjectResult(true);
    }

    [Function("CleanOldUsersHttp")]
    public async Task CleanOldUsersHttp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
    {
        await _userService.DeleteOldUsers();
    }

    // Will run every day at 4:30am, refreshing food db, in compliance with https://platform.fatsecret.com/api/Default.aspx?screen=tou 1.5
    // [Function("CleanOldUsers")]
    // public async Task CleanOldUsers([TimerTrigger("0 0 1 * *")]TimerInfo myTimer)
    // {
    //     if (myTimer.IsPastDue)
    //     {
    //         log.LogInformation("Timer is running late!");
    //     }
    //     log.LogInformation("C# Timer trigger function executed at: {Now}", DateTime.Now);
    //     
    //     await _userService.DeleteOldUsers();
    // }
}