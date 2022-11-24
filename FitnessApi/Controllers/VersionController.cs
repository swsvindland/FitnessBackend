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

public sealed class VersionController
{
    private readonly IAuthService _authService;
    
    public VersionController(IAuthService authService)
    {
        _authService = authService;
    }

    [FunctionName("MinVersion")]
    public async Task<IActionResult> MinVersion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        var authed = await _authService.CheckAuth(req);
        
        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        const int minVersion = 24;
        
        return new OkObjectResult(minVersion);
    }
    
}