using ExpenseTrackerAPI.Models;

namespace ExpenseTrackerAPI.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetUserTransactionsByMonthAsync(Guid userId, int year, int month);
        Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(Guid userId, int count);
        Task<Transaction?> GetTransactionByIdAsync(Guid id, Guid userId);
        Task AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(Transaction transaction);
    }
}
