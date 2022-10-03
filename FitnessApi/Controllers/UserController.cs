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

    public UserController(IUserService userService)
    {
        _userService = userService;
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

        var token = await _userService.AuthByEmailPassword(data.Email, data.Password);
        return new OkObjectResult(token);
    }

    [FunctionName("GetUser")]
    public async Task<IActionResult> GetUser(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        string email = req.Query["email"];

        var user = await _userService.GetUserByEmail(email);

        return new OkObjectResult(user);
    }
}