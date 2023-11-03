using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FitnessApi.Controllers;

public sealed class VersionController
{
    private readonly IAuthService _authService;
    private readonly ILogger<VersionController> _logger;

    public VersionController(IAuthService authService, ILogger<VersionController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [Function("MinVersion")]
    public IActionResult MinVersion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        const int minVersion = 34;

        return new OkObjectResult(minVersion);
    }
}