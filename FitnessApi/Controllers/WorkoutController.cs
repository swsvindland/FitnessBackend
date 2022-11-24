using System;
using System.IO;
using System.Threading.Tasks;
using FitnessRepository.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers;

public sealed class WorkoutController
{
    private readonly IWorkoutService _workoutService;
    private readonly IAuthService _authService;

    public WorkoutController(IWorkoutService workoutService, IAuthService authService)
    {
        _workoutService = workoutService;
        _authService = authService;
    }

    [FunctionName("GetExercises")]
    public async Task<IActionResult> GetExercises(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var exercises = await _workoutService.GetExercises();

        return new OkObjectResult(exercises);
    }

    [FunctionName("GetWorkouts")]
    public async Task<IActionResult> GetWorkouts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var workouts = await _workoutService.GetWorkouts();

        return new OkObjectResult(workouts);
    }

    [FunctionName("GetWorkout")]
    public async Task<IActionResult> GetWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var workoutId = long.Parse(req.Query["workoutId"]);

        var workouts = await _workoutService.GetWorkout(workoutId);

        return new OkObjectResult(workouts);
    }
    
    [FunctionName("GetWorkoutExercises")]
    public async Task<IActionResult> GetWorkoutExercises(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var workoutId = long.Parse(req.Query["workoutId"]);
        var day = int.Parse(req.Query["day"]);

        var workouts = await _workoutService.GetWorkoutExercises(workoutId, day);

        return new OkObjectResult(workouts);
    }

    [FunctionName("GetUserWorkouts")]
    public async Task<IActionResult> GetUserWorkouts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var workouts = await _workoutService.GetUserWorkouts(userId);

        return new OkObjectResult(workouts);
    }

    [FunctionName("BuyWorkout")]
    public async Task<IActionResult> BuyWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.BuyWorkout(userId, workoutId);

        return new OkObjectResult(true);
    }

    [FunctionName("SetWorkoutActive")]
    public async Task<IActionResult> SetWorkoutActive(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.SetActiveWorkout(userId, workoutId);

        return new OkObjectResult(true);
    }

    [FunctionName("GetUserWorkoutActivities")]
    public async Task<IActionResult> GetUserWorkoutActivities(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutBlockExerciseId = long.Parse(req.Query["workoutBlockExerciseId"]);

        var activity = await _workoutService.GetUserWorkoutActivities(userId, workoutBlockExerciseId);

        return new OkObjectResult(activity);
    }

    [FunctionName("GetUserWorkoutActivity")]
    public async Task<IActionResult> GetUserWorkoutActivity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutBlockExerciseId = long.Parse(req.Query["workoutBlockExerciseId"]);
        var set = int.Parse(req.Query["set"]);
        var week = int.Parse(req.Query["week"]);
        var day = int.Parse(req.Query["day"]);

        var activity = await _workoutService.GetUserWorkoutActivity(userId, workoutBlockExerciseId, set, week, day);

        return new OkObjectResult(activity);
    }

    [FunctionName("AddUserWorkoutActivity")]
    public async Task<IActionResult> AddUserWorkoutActivity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserWorkoutActivity>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        await _workoutService.AddUserWorkoutActivity(data);

        return new OkObjectResult(true);
    }

    [FunctionName("AddUserWorkoutCompleted")]
    public async Task<IActionResult> AddUserWorkoutCompleted(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserWorkoutsCompleted>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        await _workoutService.UserCompleteWorkout(data);

        return new OkObjectResult(true);
    }

    [FunctionName("GetNextWorkout")]
    public async Task<IActionResult> GetNextWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var nextWorkout = await _workoutService.GetUserNextWorkout(userId);

        return new OkObjectResult(nextWorkout);
    }

    [FunctionName("RestartWorkout")]
    public async Task<IActionResult> RestartWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var authed = await _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.RestartWorkout(userId, workoutId);

        return new OkObjectResult(true);
    }
}