using ExpenseTrackerAPI.DTOs.Transaction;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Models.Enums;
using ExpenseTrackerAPI.Repositories.Interfaces;
using ExpenseTrackerAPI.Services.Interfaces;

namespace ExpenseTrackerAPI.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TransactionDto>> GetUserTransactionsByMonthAsync(Guid userId, int year, int month)
        {
            var transactions = await _transactionRepository.GetUserTransactionsByMonthAsync(userId, year, month);
            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Date = t.Date,
                Note = t.Note,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                Type = t.Category.Type,
                CategoryIcon = t.Category.Icon,
                CategoryColor = t.Category.Color
            });
        }

        public async Task<IEnumerable<TransactionDto>> GetRecentTransactionsAsync(Guid userId, int count)
        {
            var transactions = await _transactionRepository.GetRecentTransactionsAsync(userId, count);
            return transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Date = t.Date,
                Note = t.Note,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                Type = t.Category.Type,
                CategoryIcon = t.Category.Icon,
                CategoryColor = t.Category.Color
            });
        }

        public async Task<TransactionDto?> GetTransactionByIdAsync(Guid id, Guid userId)
        {
            var t = await _transactionRepository.GetTransactionByIdAsync(id, userId);
            if (t == null) return null;

            return new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Date = t.Date,
                Note = t.Note,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                Type = t.Category.Type,
                CategoryIcon = t.Category.Icon,
                CategoryColor = t.Category.Color
            };
        }

        public async Task<TransactionDto> CreateTransactionAsync(Guid userId, CreateTransactionDto dto)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId, userId);
            if (category == null) throw new Exception("Danh mục không tồn tại.");

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("Người dùng không tồn tại.");

            var transaction = new Transaction
            {
                Amount = dto.Amount,
                Date = dto.Date,
                Note = dto.Note,
                CategoryId = dto.CategoryId,
                UserId = userId
            };

            // Bù trừ số dư hiện có
            if (category.Type == TransactionType.Income)
                user.CurrentBalance += dto.Amount;
            else
                user.CurrentBalance -= dto.Amount;

            await _transactionRepository.AddTransactionAsync(transaction);
            await _userRepository.UpdateUserAsync(user);
            
            // LƯU TOÀN BỘ VÀO DATABASE CÙNG 1 LÚC -> Đảm bảo Data Consistency (Atomic)
            await _unitOfWork.SaveChangesAsync();

            return new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Note = transaction.Note,
                CategoryId = transaction.CategoryId,
                CategoryName = category.Name,
                Type = category.Type,
                CategoryIcon = category.Icon,
                CategoryColor = category.Color
            };
        }

        public async Task<TransactionDto?> UpdateTransactionAsync(Guid id, Guid userId, UpdateTransactionDto dto)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id, userId);
            if (transaction == null) return null;

            var oldCategory = transaction.Category;
            var newCategory = await _categoryRepository.GetCategoryByIdAsync(dto.CategoryId, userId);
            if (newCategory == null) throw new Exception("Danh mục mới không tồn tại.");

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("Người dùng không tồn tại.");

            // 1. Hoàn tác số tiền cũ
            if (oldCategory.Type == TransactionType.Income)
                user.CurrentBalance -= transaction.Amount;
            else
                user.CurrentBalance += transaction.Amount;

            // 2. Cập nhật thông tin giao dịch
            transaction.Amount = dto.Amount;
            transaction.Date = dto.Date;
            transaction.Note = dto.Note;
            transaction.CategoryId = dto.CategoryId;

            // 3. Áp dụng số tiền mới
            if (newCategory.Type == TransactionType.Income)
                user.CurrentBalance += dto.Amount;
            else
                user.CurrentBalance -= dto.Amount;

            await _transactionRepository.UpdateTransactionAsync(transaction);
            await _userRepository.UpdateUserAsync(user);
            
            // Lưu toàn bộ đồng thời
            await _unitOfWork.SaveChangesAsync();

            return new TransactionDto
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Note = transaction.Note,
                CategoryId = transaction.CategoryId,
                CategoryName = newCategory.Name,
                Type = newCategory.Type,
                CategoryIcon = newCategory.Icon,
                CategoryColor = newCategory.Color
            };
        }

        public async Task<bool> DeleteTransactionAsync(Guid id, Guid userId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(id, userId);
            if (transaction == null) return false;

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            // Hoàn tác số dư
            if (transaction.Category.Type == TransactionType.Income)
                user.CurrentBalance -= transaction.Amount;
            else
                user.CurrentBalance += transaction.Amount;

            await _transactionRepository.DeleteTransactionAsync(transaction);
            await _userRepository.UpdateUserAsync(user);
            
            // Lưu toàn bộ đồng thời
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
