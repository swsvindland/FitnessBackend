using FitnessRepository.Models;
using FitnessServices.Models;

namespace FitnessServices.Services;

public class DashboardService: IDashboardService
{
    private readonly IBodyService _bodyService;
    private readonly IWorkoutService _workoutService;
    
    public DashboardService(IBodyService bodyService, IWorkoutService workoutService)
    {
        _bodyService = bodyService;
        _workoutService = workoutService;
    }
    
    public async Task<Dashboard> GetUserDashboard(Guid userId, DateTime date)
    {
        var userHeights = await _bodyService.GetAllUserHeights(userId);
        var userWeights = await _bodyService.GetAllUserWeights(userId);
        var userBodies = await _bodyService.GetAllUserBodies(userId);
        var userBloodPressure = await _bodyService.GetAllUserBloodPressures(userId);
        var userWorkouts = await _workoutService.GetUserWorkoutsCompleted(userId);

        return new Dashboard()
        {
            AddHeight = !userHeights.Any(),
            AddWeight = userWeights.LastOrDefault()?.Created.Date != date.Date,
            AddBloodPressure = (userBloodPressure.LastOrDefault()?.Created ?? DateTime.Now) < date.AddDays(-7),
            AddBodyMeasurements = (userBodies.LastOrDefault()?.Created ?? DateTime.Now) < date.AddDays(-7),
            DoWorkout = userWorkouts.LastOrDefault()?.Created.Date != date.Date
        };
    }
}