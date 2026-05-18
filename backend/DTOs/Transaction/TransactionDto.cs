using ExpenseTrackerAPI.Models.Enums;

namespace ExpenseTrackerAPI.DTOs.Transaction
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
        
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public TransactionType Type { get; set; }
        public string? CategoryIcon { get; set; }
        public string? CategoryColor { get; set; }
    }
}
