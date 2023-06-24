using FitnessRepository.Models;
using FitnessServices.Models;

namespace FitnessServices.Services;

public sealed class DashboardService: IDashboardService
{
    private readonly IUserService _userService;
    private readonly IBodyService _bodyService;
    private readonly IWorkoutService _workoutService;
    private readonly IFoodService _foodService;
    private readonly ISupplementService _supplementService;
    
    public DashboardService(IUserService userService, IBodyService bodyService, IWorkoutService workoutService, IFoodService foodService, ISupplementService supplementService)
    {
        _userService = userService;
        _bodyService = bodyService;
        _workoutService = workoutService;
        _foodService = foodService;
        _supplementService = supplementService;
    }
    
    public async Task<Dashboard> GetUserDashboard(Guid userId, DateTime date)
    {
        var user = await _userService.GetUserById(userId);
        var userHeights = (await _bodyService.GetAllUserHeights(userId)).ToArray();
        var userWeights = (await _bodyService.GetAllUserWeights(userId)).ToArray();
        var userWorkouts = (await _workoutService.GetUserWorkoutsCompleted(userId)).ToArray();
        var goalMacros = await _foodService.GetUserMacros(userId);
        var userMacros = await _foodService.GetUserCurrentMacosV2(userId, date);
        var supplements = (await _supplementService.GetUserSupplements(userId)).ToArray();
        var supplementActivity = (await _supplementService.GetUserSupplementActivitiesByDate(userId, date)).ToArray();
        var checkIn = await _bodyService.GetLastUserCheckIn(userId);

        return new Dashboard()
        {
            AddSex = user?.Sex == Sex.Unknown,
            AddHeight = !userHeights.Any(),
            HeightAdded = userHeights.Any(e => e.Created.Date == date.Date),
            AddWeight = userWeights.LastOrDefault()?.Created.Date != date.Date,
            WeightAdded = userWeights.Any(e => e.Created.Date == date.Date),
            DoWorkout = userWorkouts.LastOrDefault()?.Created.Date != date.Date,
            WorkoutAdded = userWorkouts.Any(e => e.Created.Date == date.Date),
            TrackMacros = true,
            MacrosAdded = (Math.Abs((goalMacros?.Calories ?? 0) - userMacros.Calories) / (goalMacros?.Calories ?? 1)) < 0.1,
            AddSupplements = supplements.Length == 0,
            SupplementsAdded = supplements.Any(e => e.Created.Date == date.Date),
            TrackSupplements = supplements.Any(),
            SupplementsTracked = supplements.Length <= supplementActivity.Length,
            AddCheckIn = checkIn == null || checkIn.Created.Date < date.AddDays(-6),
            CheckInAdded = checkIn?.Created.Date == date.Date
        };
    }
    
    public async Task<CheckIn> GetUserCheckIn(Guid userId, DateTime date)
    {
        var userBodies = (await _bodyService.GetAllUserBodies(userId)).ToArray();
        var userBloodPressure = (await _bodyService.GetAllUserBloodPressures(userId)).ToArray();
        var photos = (await _bodyService.GetProgressPhotos(userId)).ToArray();
        

        return new CheckIn()
        {
            BloodPressureAdded = userBloodPressure.Any(e => e.Created.Date == date.Date),
            BodyMeasurementsAdded = userBodies.Any(e => e.Created.Date == date.Date),
            ProgressPhotosAdded = photos.Any(e => e.Created.Date == date.Date)
        };
    }
}