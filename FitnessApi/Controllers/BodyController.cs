using System;
using System.IO;
using System.Threading.Tasks;
using FitnessRepository.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers;

public sealed class BodyController
{
    private readonly IBodyService _bodyService;
    private readonly IAuthService _authService;
    private readonly ILogger<BodyController> _logger;


    public BodyController(IBodyService bodyService, IAuthService authService, ILogger<BodyController> logger)
    {
        _bodyService = bodyService;
        _authService = authService;
        _logger = logger;
    }

    [Function("GetUserWeights")]
    public async Task<IActionResult> GetUserWeights(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GetAllUserWeights(userId);

        return new OkObjectResult(user);
    }

    [Function("AddUserWeight")]
    public async Task<IActionResult> AddUserWeight(
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

        var data = JsonConvert.DeserializeObject<UserWeight>(requestBody);

        await _bodyService.AddUserWeight(data);

        return new OkObjectResult(true);
    }

    [Function("UpdateUserWeight")]
    public async Task<IActionResult> UpdateUserWeight(
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

        var data = JsonConvert.DeserializeObject<UserWeight>(requestBody);

        await _bodyService.UpdateUserWeight(data);

        return new OkObjectResult(true);
    }

    [Function("DeleteUserWeight")]
    public async Task<IActionResult> DeleteUserWeight(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var id = long.Parse(req.Query["id"]);

        await _bodyService.DeleteUserWeight(id);

        return new OkObjectResult(true);
    }

    [Function("GetUserBodies")]
    public async Task<IActionResult> GetUserBodies(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GetAllUserBodies(userId);

        return new OkObjectResult(user);
    }

    [Function("AddUserBody")]
    public async Task<IActionResult> AddUserBody(
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

        var data = JsonConvert.DeserializeObject<UserBody>(requestBody);

        await _bodyService.AddUserBody(data);

        return new OkObjectResult(true);
    }

    [Function("UpdateUserBody")]
    public async Task<IActionResult> UpdateUserBody(
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

        var data = JsonConvert.DeserializeObject<UserBody>(requestBody);

        await _bodyService.UpdateUserBody(data);

        return new OkObjectResult(true);
    }

    [Function("DeleteUserBody")]
    public async Task<IActionResult> DeleteUserBody(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var id = long.Parse(req.Query["id"]);

        await _bodyService.DeleteUserBody(id);

        return new OkObjectResult(true);
    }

    [Function("GetUserBloodPressures")]
    public async Task<IActionResult> GetUserBloodPressures(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GetAllUserBloodPressures(userId);

        return new OkObjectResult(user);
    }

    [Function("GetUserBodyFat")]
    public async Task<IActionResult> GetUserBodyFat(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GenerateBodyFats(userId);

        return new OkObjectResult(user);
    }

    [Function("AddUserBloodPressure")]
    public async Task<IActionResult> AddUserBloodPressure(
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

        var data = JsonConvert.DeserializeObject<UserBloodPressure>(requestBody);

        await _bodyService.AddUserBloodPressure(data);

        return new OkObjectResult(true);
    }

    [Function("UpdateUserBloodPressure")]
    public async Task<IActionResult> UpdateUserBloodPressure(
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

        var data = JsonConvert.DeserializeObject<UserBloodPressure>(requestBody);

        await _bodyService.UpdateUserBloodPressure(data);

        return new OkObjectResult(true);
    }

    [Function("DeleteUserBloodPressure")]
    public async Task<IActionResult> DeleteUserBloodPressure(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var id = long.Parse(req.Query["id"]);

        await _bodyService.DeleteUserBloodPressure(id);

        return new OkObjectResult(true);
    }

    [Function("AddUserHeight")]
    public async Task<IActionResult> AddUserHeight(
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

        var data = JsonConvert.DeserializeObject<UserHeight>(requestBody);

        await _bodyService.AddUserHeight(data);

        return new OkObjectResult(true);
    }

    [Function("UploadProgressPhoto")]
    public async Task<IActionResult> UploadProgressPhoto(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }


        var connection = Environment.GetEnvironmentVariable("FileUploadStorage");
        var containerName = Environment.GetEnvironmentVariable("ContainerName");

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);
        var file = req.Form.Files["file"];

        if (connection == null) throw new Exception("Missing connection string");
        if (containerName == null) throw new Exception("Missing container name");
        if (file == null) return null;

        var filePath = await _bodyService.UploadProgressPhoto(userId, date, file, connection, containerName);

        return new OkObjectResult(filePath);
    }

    [Function("GetProgressPhotos")]
    public async Task<IActionResult> GetProgressPhotos(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var filePath = await _bodyService.GetProgressPhotos(userId);

        return new OkObjectResult(filePath);
    }
}