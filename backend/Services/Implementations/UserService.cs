using ExpenseTrackerAPI.Repositories.Interfaces;
using ExpenseTrackerAPI.Services.Interfaces;
using ExpenseTrackerAPI.DTOs.User;
namespace ExpenseTrackerAPI.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("Không tìm thấy người dùng.");

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                CurrentBalance = user.CurrentBalance
            };
        }

        public async Task<decimal> GetCurrentBalanceAsync(Guid userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("Không tìm thấy User.");

            return user.CurrentBalance;
        }

        public async Task<bool> UpdateBalanceAsync(Guid userId, decimal newBalance)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            user.CurrentBalance = newBalance;
            await _userRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
