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
            AddWeight = userWeights.All(e => e.Created.Date != date.Date),
            AddBloodPressure = !userBloodPressure.Any(e => e.Created.AddDays(-7) < date),
            AddBodyMeasurements = !userBodies.Any(e => e.Created.AddDays(-7) < date),
            DoWorkout = userWorkouts.Any(e => e.Created.AddDays(-1) <= date),
        };
    }
}