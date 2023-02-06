using System.Threading.Tasks;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FitnessApi.Controllers;

public sealed class VersionController
{

    public VersionController()
    {
    }

    [FunctionName("MinVersion")]
    public IActionResult MinVersion(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req,
        ILogger log)
    {
        const int minVersion = 33;

        return new OkObjectResult(minVersion);
    }
}