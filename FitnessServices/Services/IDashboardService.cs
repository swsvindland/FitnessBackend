using FitnessServices.Models;

namespace FitnessServices.Services;

public interface IDashboardService
{
    Task<Dashboard> GetUserDashboard(Guid userId, DateTime date);
    Task<CheckIn> GetUserCheckIn(Guid userId, DateTime date);
}