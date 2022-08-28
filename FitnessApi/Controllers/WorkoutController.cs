using System.Threading.Tasks;
using FitnessServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FitnessApi.Controllers;

public class WorkoutController
{
    private readonly IWorkoutService _workoutService;

    public WorkoutController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }
    
    [FunctionName("GetWorkouts")]
    public async Task<IActionResult> GetWorkouts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
    {
        var workouts = await _workoutService.GetWorkouts();

        return new OkObjectResult(workouts);
    }
    
    [FunctionName("GetWorkout")]
    public async Task<IActionResult> GetWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
    {
        var workoutId = long.Parse(req.Query["workoutId"]);
        
        var workouts = await _workoutService.GetWorkout(workoutId);

        return new OkObjectResult(workouts);
    }
    
    [FunctionName("GetWorkoutDetails")]
    public async Task<IActionResult> GetWorkoutDetails(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
    {
        var workoutId = long.Parse(req.Query["workoutId"]);
        
        var workouts = await _workoutService.GetWorkoutBlocks(workoutId);

        return new OkObjectResult(workouts);
    }
}