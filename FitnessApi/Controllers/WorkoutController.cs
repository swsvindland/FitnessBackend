using System;
using System.IO;
using System.Threading.Tasks;
using FitnessRepository.Models;
using FitnessServices.Models;
using FitnessServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitnessApi.Controllers;

public sealed class WorkoutController
{
    private readonly IWorkoutService _workoutService;
    private readonly IAuthService _authService;
    private readonly ILogger<WorkoutController> _logger;

    public WorkoutController(IWorkoutService workoutService, IAuthService authService,
        ILogger<WorkoutController> logger)
    {
        _workoutService = workoutService;
        _authService = authService;
        _logger = logger;
    }

    [Function("GetExercises")]
    public async Task<IActionResult> GetExercises(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var exercises = await _workoutService.GetExercises();

        return new OkObjectResult(exercises);
    }

    [Function("GetWorkouts")]
    public async Task<IActionResult> GetWorkouts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var workouts = await _workoutService.GetWorkouts();

        return new OkObjectResult(workouts);
    }

    [Function("GetWorkout")]
    public async Task<IActionResult> GetWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var workoutId = long.Parse(req.Query["workoutId"]);

        var workouts = await _workoutService.GetWorkout(workoutId);

        return new OkObjectResult(workouts);
    }

    [Function("GetWorkoutExercises")]
    public async Task<IActionResult> GetWorkoutExercises(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var workoutId = long.Parse(req.Query["workoutId"]);
        var day = int.Parse(req.Query["day"]);

        var workouts = await _workoutService.GetWorkoutExercises(workoutId, day);

        return new OkObjectResult(workouts);
    }

    [Function("GetAllWorkoutExercises")]
    public async Task<IActionResult> GetAllWorkoutExercises(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var workoutId = long.Parse(req.Query["workoutId"]);

        var workouts = await _workoutService.GetWorkoutExercises(workoutId);

        return new OkObjectResult(workouts);
    }

    [Function("GetUserWorkouts")]
    public async Task<IActionResult> GetUserWorkouts(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var workouts = await _workoutService.GetUserWorkouts(userId);

        return new OkObjectResult(workouts);
    }

    [Function("BuyWorkout")]
    public async Task<IActionResult> BuyWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.BuyWorkout(userId, workoutId);

        return new OkObjectResult(true);
    }

    [Function("SetWorkoutActive")]
    public async Task<IActionResult> SetWorkoutActive(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.SetActiveWorkout(userId, workoutId);

        return new OkObjectResult(true);
    }

    [Function("GetUserWorkoutExercise")]
    public async Task<IActionResult> GetUserWorkoutExercise(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutExerciseId = long.Parse(req.Query["workoutExerciseId"]);
        var week = int.Parse(req.Query["week"]);
        var day = int.Parse(req.Query["day"]);

        var activity = await _workoutService.GetUserWorkoutExercise(userId, workoutExerciseId, week, day);

        return new OkObjectResult(activity);
    }

    [Function("GetUserWorkoutActivities")]
    public async Task<IActionResult> GetUserWorkoutActivities(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutExerciseId = long.Parse(req.Query["workoutExerciseId"]);

        var activity = await _workoutService.GetUserWorkoutActivities(userId, workoutExerciseId);

        return new OkObjectResult(activity);
    }

    [Function("AddUserWorkoutActivity")]
    public async Task<IActionResult> AddUserWorkoutActivity(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

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

    [Function("AddUserWorkoutCompleted")]
    public async Task<IActionResult> AddUserWorkoutCompleted(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

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

    [Function("GetNextWorkout")]
    public async Task<IActionResult> GetNextWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var nextWorkout = await _workoutService.GetUserNextWorkout(userId);

        return new OkObjectResult(nextWorkout);
    }

    [Function("RestartWorkout")]
    public async Task<IActionResult> RestartWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.RestartWorkout(userId, workoutId);

        return new OkObjectResult(true);
    }

    [Function("GetWorkoutsByUserId")]
    public async Task<IActionResult> GetWorkoutsByUserId(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);

        var customWorkouts = await _workoutService.GetWorkoutsByUserId(userId);

        return new OkObjectResult(customWorkouts);
    }

    [Function("AddWorkout")]
    public async Task<IActionResult> AddWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<Workout>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        var id = await _workoutService.AddWorkout(data);

        return new OkObjectResult(id);
    }

    [Function("EditWorkout")]
    public async Task<IActionResult> EditWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<Workout>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        var id = await _workoutService.EditWorkout(data);

        return new OkObjectResult(id);
    }

    [Function("DeleteWorkout")]
    public async Task<IActionResult> DeleteUserCustomWorkout(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.DeleteWorkout(workoutId);

        return new OkObjectResult(true);
    }


    [Function("UpsertWorkoutExercises")]
    public async Task<IActionResult> UpsertWorkoutExercises(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UpdateWorkoutExercise>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        var workoutExerciseId = await _workoutService.UpsertWorkoutExercise(data);

        return new OkObjectResult(workoutExerciseId);
    }

    [Function("GetUserWorkoutSubstitution")]
    public async Task<IActionResult> GetUserWorkoutSubstitution(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var userId = Guid.Parse(req.Query["userId"]);
        var workoutExerciseId = long.Parse(req.Query["workoutExerciseId"]);

        var userWorkoutSubstitution = await _workoutService.GetUserWorkoutSubstitution(userId, workoutExerciseId);

        return new OkObjectResult(userWorkoutSubstitution);
    }

    [Function("AddUserWorkoutSubstitution")]
    public async Task<IActionResult> AddUserWorkoutSubstitution(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserWorkoutSubstitution>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        var id = await _workoutService.AddUserWorkoutSubstitution(data);

        return new OkObjectResult(id);
    }

    [Function("UpdateUserWorkoutSubstitution")]
    public async Task<IActionResult> UpdateUserWorkoutSubstitution(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        string requestBody;
        using (var streamReader = new StreamReader(req.Body))
        {
            requestBody = await streamReader.ReadToEndAsync();
        }

        var data = JsonConvert.DeserializeObject<UserWorkoutSubstitution>(requestBody);

        if (data == null)
        {
            return new BadRequestResult();
        }

        await _workoutService.UpdateUserWorkoutSubstitution(data);

        return new OkObjectResult(true);
    }

    [Function("DeleteUserWorkoutSubstitution")]
    public async Task<IActionResult> DeleteUserWorkoutSubstitution(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)]
        HttpRequest req)
    {
        var authed = _authService.CheckAuth(req);

        if (authed == false)
        {
            return new UnauthorizedResult();
        }

        var id = long.Parse(req.Query["id"]);

        await _workoutService.DeleteUserWorkoutSubstitution(id);

        return new OkObjectResult(true);
    }
}