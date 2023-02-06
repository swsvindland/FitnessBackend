using System;
using System.IO;
using System.Threading.Tasks;
using FitnessRepository.Models;
using FitnessServices.Models;
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

    public WorkoutController(IWorkoutService workoutService)
    {
        _workoutService = workoutService;
    }

    [FunctionName("GetExercises")]
    public async Task<IActionResult> GetExercises(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var exercises = await _workoutService.GetExercises();

        return new OkObjectResult(exercises);
    }

    [FunctionName("GetWorkouts")]
    public async Task<IActionResult> GetWorkouts(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var workouts = await _workoutService.GetWorkouts();

        return new OkObjectResult(workouts);
    }
    
    [FunctionName("GetCardioWorkouts")]
    public async Task<IActionResult> GetCardioWorkouts(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var workouts = await _workoutService.GetCardioWorkouts();

        return new OkObjectResult(workouts);
    }

    [FunctionName("GetWorkout")]
    public async Task<IActionResult> GetWorkout(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var workoutId = long.Parse(req.Query["workoutId"]);

        var workouts = await _workoutService.GetWorkout(workoutId);

        return new OkObjectResult(workouts);
    }

    [FunctionName("GetWorkoutExercises")]
    public async Task<IActionResult> GetWorkoutExercises(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var workoutId = long.Parse(req.Query["workoutId"]);
        var day = int.Parse(req.Query["day"]);

        var workouts = await _workoutService.GetWorkoutExercises(workoutId, day);

        return new OkObjectResult(workouts);
    }
    
    [FunctionName("GetAllWorkoutExercises")]
    public async Task<IActionResult> GetAllWorkoutExercises(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var workoutId = long.Parse(req.Query["workoutId"]);

        var workouts = await _workoutService.GetWorkoutExercises(workoutId);

        return new OkObjectResult(workouts);
    }

    [FunctionName("GetUserWorkouts")]
    public async Task<IActionResult> GetUserWorkouts(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);

        var workouts = await _workoutService.GetUserWorkouts(userId);

        return new OkObjectResult(workouts);
    }

    [FunctionName("BuyWorkout")]
    public async Task<IActionResult> BuyWorkout(
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.BuyWorkout(userId, workoutId);

        return new OkObjectResult(true);
    }

    [FunctionName("SetWorkoutActive")]
    public async Task<IActionResult> SetWorkoutActive(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.SetActiveWorkout(userId, workoutId);

        return new OkObjectResult(true);
    }
    
    [FunctionName("GetUserWorkoutExercise")]
    public async Task<IActionResult> GetUserWorkoutExercise(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var workoutExerciseId = long.Parse(req.Query["workoutExerciseId"]);
        var week = int.Parse(req.Query["week"]);
        var day = int.Parse(req.Query["day"]);
        
        var activity = await _workoutService.GetUserWorkoutExercise(userId, workoutExerciseId, week, day);

        return new OkObjectResult(activity);
    }

    [FunctionName("GetUserWorkoutActivities")]
    public async Task<IActionResult> GetUserWorkoutActivities(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var workoutExerciseId = long.Parse(req.Query["workoutExerciseId"]);

        var activity = await _workoutService.GetUserWorkoutActivities(userId, workoutExerciseId);

        return new OkObjectResult(activity);
    }

    [FunctionName("GetUserWorkoutActivity")]
    public async Task<IActionResult> GetUserWorkoutActivity(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var workoutExerciseId = long.Parse(req.Query["workoutExerciseId"]);
        var set = int.Parse(req.Query["set"]);
        var week = int.Parse(req.Query["week"]);
        var day = int.Parse(req.Query["day"]);

        var activity = await _workoutService.GetUserWorkoutActivity(userId, workoutExerciseId, set, week, day);

        return new OkObjectResult(activity);
    }

    [FunctionName("AddUserWorkoutActivity")]
    public async Task<IActionResult> AddUserWorkoutActivity(
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
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
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
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
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);

        var nextWorkout = await _workoutService.GetUserNextWorkout(userId);

        return new OkObjectResult(nextWorkout);
    }
    
    [FunctionName("GetNextCardioWorkout")]
    public async Task<IActionResult> GetNextCardioWorkout(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);

        var nextWorkout = await _workoutService.GetUserNextCardioWorkout(userId);

        return new OkObjectResult(nextWorkout);
    }

    [FunctionName("RestartWorkout")]
    public async Task<IActionResult> RestartWorkout(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.RestartWorkout(userId, workoutId);

        return new OkObjectResult(true);
    }
    
    [FunctionName("GetWorkoutsByUserId")]
    public async Task<IActionResult> GetWorkoutsByUserId(
        [HttpTrigger(AuthorizationLevel.User, "get", Route = null)]
        HttpRequest req, ILogger log)
    {
        var userId = Guid.Parse(req.Query["userId"]);

        var customWorkouts = await _workoutService.GetWorkoutsByUserId(userId);

        return new OkObjectResult(customWorkouts);
    }

    [FunctionName("AddWorkout")]
    public async Task<IActionResult> AddWorkout(
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
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

    [FunctionName("EditWorkout")]
    public async Task<IActionResult> EditWorkout(
        [HttpTrigger(AuthorizationLevel.User, "put", Route = null)]
        HttpRequest req, ILogger log)
    {
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

    [FunctionName("DeleteWorkout")]
    public async Task<IActionResult> DeleteUserCustomWorkout(
        [HttpTrigger(AuthorizationLevel.User, "delete", Route = null)]
        HttpRequest req, ILogger log)
    {
        var workoutId = long.Parse(req.Query["workoutId"]);

        await _workoutService.DeleteWorkout(workoutId);

        return new OkObjectResult(true);
    }



    [FunctionName("UpsertWorkoutExercises")]
    public async Task<IActionResult> UpsertWorkoutExercises(
        [HttpTrigger(AuthorizationLevel.User, "post", Route = null)]
        HttpRequest req, ILogger log)
    {
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
}