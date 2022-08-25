using System;
using System.Threading.Tasks;
using FitnessServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FitnessApi.Controllers;

public class FoodController
{
    private readonly IFoodService _foodService;

    public FoodController(IFoodService foodService)
    {
        _foodService = foodService;
    }
    
    [FunctionName("GetMacros")]
    public async Task<IActionResult> GetMacros(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        
        var user = await _foodService.GenerateMacros(userId);

        return new OkObjectResult(user);
    }
}