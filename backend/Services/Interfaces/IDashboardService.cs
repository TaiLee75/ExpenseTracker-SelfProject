using ExpenseTrackerAPI.DTOs.Dashboard;

namespace ExpenseTrackerAPI.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetMonthlySummaryAsync(Guid userId, int year, int month);
    }
}
