using System;
using System.Threading.Tasks;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FitnessApi.Controllers;

public sealed class DashboardController
{
    private readonly IDashboardService _dashboardService;
    private readonly IAuthService _authService;

    public DashboardController(IDashboardService dashboardService, IAuthService authService)
    {
        _dashboardService = dashboardService;
        _authService = authService;
    }

    [FunctionName("GetUserDashboard")]
    public async Task<IActionResult> GetUserDashboard(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var dashboard = await _dashboardService.GetUserDashboard(userId, date);

        return new OkObjectResult(dashboard);
    }
    
    [FunctionName("GetUserCheckIn")]
    public async Task<IActionResult> GetUserCheckIn(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var date = DateTime.Parse(req.Query["date"]);

        var checkIn = await _dashboardService.GetUserCheckIn(userId, date);

        return new OkObjectResult(checkIn);
    }
}