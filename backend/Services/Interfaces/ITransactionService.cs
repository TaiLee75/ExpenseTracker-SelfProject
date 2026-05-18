using ExpenseTrackerAPI.DTOs.Transaction;

namespace ExpenseTrackerAPI.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetUserTransactionsByMonthAsync(Guid userId, int year, int month);
        Task<IEnumerable<TransactionDto>> GetRecentTransactionsAsync(Guid userId, int count);
        Task<TransactionDto?> GetTransactionByIdAsync(Guid id, Guid userId);
        Task<TransactionDto> CreateTransactionAsync(Guid userId, CreateTransactionDto dto);
        Task<TransactionDto?> UpdateTransactionAsync(Guid id, Guid userId, UpdateTransactionDto dto);
        Task<bool> DeleteTransactionAsync(Guid id, Guid userId);
    }
}
