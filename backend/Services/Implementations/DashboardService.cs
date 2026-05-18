using ExpenseTrackerAPI.DTOs.Dashboard;
using ExpenseTrackerAPI.Models.Enums;
using ExpenseTrackerAPI.Repositories.Interfaces;
using ExpenseTrackerAPI.Services.Interfaces;

namespace ExpenseTrackerAPI.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly ITransactionRepository _transactionRepository;

        public DashboardService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<DashboardSummaryDto> GetMonthlySummaryAsync(Guid userId, int year, int month)
        {
            var transactions = await _transactionRepository.GetUserTransactionsByMonthAsync(userId, year, month);

            var incomeTransactions = transactions.Where(t => t.Category.Type == TransactionType.Income).ToList();
            var expenseTransactions = transactions.Where(t => t.Category.Type == TransactionType.Expense).ToList();

            var totalIncome = incomeTransactions.Sum(t => t.Amount);
            var totalExpense = expenseTransactions.Sum(t => t.Amount);

            var incomeBreakdown = incomeTransactions
                .GroupBy(t => t.Category)
                .Select(g => new CategoryBreakdownDto
                {
                    CategoryId = g.Key.Id,
                    CategoryName = g.Key.Name,
                    CategoryColor = g.Key.Color,
                    TotalAmount = g.Sum(t => t.Amount),
                    Percentage = totalIncome > 0 ? Math.Round((double)(g.Sum(t => t.Amount) / totalIncome) * 100, 2) : 0
                })
                .OrderByDescending(b => b.TotalAmount)
                .ToList();

            var expenseBreakdown = expenseTransactions
                .GroupBy(t => t.Category)
                .Select(g => new CategoryBreakdownDto
                {
                    CategoryId = g.Key.Id,
                    CategoryName = g.Key.Name,
                    CategoryColor = g.Key.Color,
                    TotalAmount = g.Sum(t => t.Amount),
                    Percentage = totalExpense > 0 ? Math.Round((double)(g.Sum(t => t.Amount) / totalExpense) * 100, 2) : 0
                })
                .OrderByDescending(b => b.TotalAmount)
                .ToList();

            return new DashboardSummaryDto
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                IncomeBreakdown = incomeBreakdown,
                ExpenseBreakdown = expenseBreakdown
            };
        }
    }
}
