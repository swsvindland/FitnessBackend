using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FitnessApi.Controllers;

public sealed class VersionController
{
    private readonly ILogger<VersionController> _logger;

    public VersionController(ILogger<VersionController> logger)
    {
        _logger = logger;
    }

    [Function("MinVersion")]
    public IActionResult MinVersion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        const int minVersion = 34;

        return new OkObjectResult(minVersion);
    }
}