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

    public BodyController(IBodyService bodyService)
    {
        _bodyService = bodyService;
    }

    [FunctionName("GetUserWeights")]
    public async Task<IActionResult> GetUserWeights(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GetAllUserWeights(userId);

        return new OkObjectResult(user);
    }

    [FunctionName("AddUserWeight")]
    public async Task<IActionResult> AddUserWeight(
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserWeight>(requestBody);

        await _bodyService.AddUserWeight(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("UpdateUserWeight")]
    public async Task<IActionResult> UpdateUserWeight(
        [HttpTrigger(AuthorizationLevel.User, "put", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserWeight>(requestBody);

        await _bodyService.UpdateUserWeight(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("DeleteUserWeight")]
    public async Task<IActionResult> DeleteUserWeight(
        [HttpTrigger(AuthorizationLevel.User, "delete", Route = null)]
        HttpRequest req, ILogger log)
    {
        var id = long.Parse(req.Query["id"]);

        await _bodyService.DeleteUserWeight(id);

        return new OkObjectResult(true);
    }

    [FunctionName("GetUserBodies")]
    public async Task<IActionResult> GetUserBodies(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GetAllUserBodies(userId);

        return new OkObjectResult(user);
    }

    [FunctionName("AddUserBody")]
    public async Task<IActionResult> AddUserBody(
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserBody>(requestBody);

        await _bodyService.AddUserBody(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("UpdateUserBody")]
    public async Task<IActionResult> UpdateUserBody(
        [HttpTrigger(AuthorizationLevel.User, "put", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserBody>(requestBody);

        await _bodyService.UpdateUserBody(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("DeleteUserBody")]
    public async Task<IActionResult> DeleteUserBody(
        [HttpTrigger(AuthorizationLevel.User, "delete", Route = null)]
        HttpRequest req, ILogger log)
    {
        var id = long.Parse(req.Query["id"]);

        await _bodyService.DeleteUserBody(id);

        return new OkObjectResult(true);
    }

    [FunctionName("GetUserBloodPressures")]
    public async Task<IActionResult> GetUserBloodPressures(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GetAllUserBloodPressures(userId);

        return new OkObjectResult(user);
    }

    [FunctionName("GetUserBodyFat")]
    public async Task<IActionResult> GetUserBodyFat(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);

        var user = await _bodyService.GenerateBodyFats(userId);

        return new OkObjectResult(user);
    }

    [FunctionName("AddUserBloodPressure")]
    public async Task<IActionResult> AddUserBloodPressure(
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserBloodPressure>(requestBody);

        await _bodyService.AddUserBloodPressure(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("UpdateUserBloodPressure")]
    public async Task<IActionResult> UpdateUserBloodPressure(
        [HttpTrigger(AuthorizationLevel.User, "put", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserBloodPressure>(requestBody);

        await _bodyService.UpdateUserBloodPressure(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("DeleteUserBloodPressure")]
    public async Task<IActionResult> DeleteUserBloodPressure(
        [HttpTrigger(AuthorizationLevel.User, "delete", Route = null)]
        HttpRequest req, ILogger log)
    {
        var id = long.Parse(req.Query["id"]);

        await _bodyService.DeleteUserBloodPressure(id);

        return new OkObjectResult(true);
    }

    [FunctionName("AddUserHeight")]
    public async Task<IActionResult> AddUserHeight(
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserHeight>(requestBody);

        await _bodyService.AddUserHeight(data);

        return new OkObjectResult(true);
    }
    
    [FunctionName("UploadProgressPhoto")]
    public async Task<IActionResult> UploadProgressPhoto(
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)] HttpRequest req, ILogger log) 
    {
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
    
    [FunctionName("GetProgressPhotos")]
    public async Task<IActionResult> GetProgressPhotos(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)] HttpRequest req, ILogger log) {
        var userId = Guid.Parse(req.Query["userId"]);

        var filePath = await _bodyService.GetProgressPhotos(userId);

        return new OkObjectResult(filePath);
    }
}