using FitnessRepository.Models;
using FitnessServices.Models;

namespace FitnessServices.Services;

public sealed class DashboardService: IDashboardService
{
    private readonly IBodyService _bodyService;
    private readonly IWorkoutService _workoutService;
    private readonly IFoodService _foodService;
    
    public DashboardService(IBodyService bodyService, IWorkoutService workoutService, IFoodService foodService)
    {
        _bodyService = bodyService;
        _workoutService = workoutService;
        _foodService = foodService;
    }
    
    public async Task<Dashboard> GetUserDashboard(Guid userId, DateTime date)
    {
        var userHeights = (await _bodyService.GetAllUserHeights(userId)).ToArray();
        var userWeights = (await _bodyService.GetAllUserWeights(userId)).ToArray();
        var userBodies = (await _bodyService.GetAllUserBodies(userId)).ToArray();
        var userBloodPressure = (await _bodyService.GetAllUserBloodPressures(userId)).ToArray();
        var userWorkouts = (await _workoutService.GetUserWorkoutsCompleted(userId)).ToArray();
        var goalMacros = await _foodService.GetUserMacros(userId);
        var userMacros = await _foodService.GetUserCurrentMacosV2(userId, date);

        return new Dashboard()
        {
            AddHeight = !userHeights.Any(),
            HeightAdded = userHeights.Any(e => e.Created.Date == date.Date),
            AddWeight = userWeights.LastOrDefault()?.Created.Date != date.Date,
            WeightAdded = userWeights.Any(e => e.Created.Date == date.Date),
            AddBloodPressure = (userBloodPressure.LastOrDefault()?.Created ?? DateTime.Now) < date.AddDays(-6),
            BloodPressureAdded = userBloodPressure.Any(e => e.Created.Date == date.Date),
            AddBodyMeasurements = (userBodies.LastOrDefault()?.Created ?? DateTime.Now) < date.AddDays(-6),
            BodyMeasurementsAdded = userBodies.Any(e => e.Created.Date == date.Date),
            DoWorkout = userWorkouts.LastOrDefault()?.Created.Date != date.Date,
            WorkoutAdded = userWorkouts.Any(e => e.Created.Date == date.Date),
            TrackMacros = true,
            MacrosAdded = (Math.Abs((goalMacros?.Calories ?? 0) - userMacros.Calories) / (goalMacros?.Calories ?? 1)) < 0.1
        };
    }
}