using ExpenseTrackerAPI.DTOs.User;

namespace ExpenseTrackerAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetUserProfileAsync(Guid userId);
        Task<decimal> GetCurrentBalanceAsync(Guid userId);
        Task<bool> UpdateBalanceAsync(Guid userId, decimal newBalance);
    }
}
