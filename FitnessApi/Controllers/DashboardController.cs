using System;
using System.Threading.Tasks;
using FitnessServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FitnessApi.Controllers;

public sealed class DashboardController
{
    private readonly IDashboardService _dashboardService;
    private readonly IAuthService _authService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, IAuthService authService,
        ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _authService = authService;
        _logger = logger;
    }

    [Function("GetUserDashboard")]
    public async Task<IActionResult> GetUserDashboard(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
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
}