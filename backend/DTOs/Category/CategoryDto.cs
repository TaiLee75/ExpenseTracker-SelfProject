using ExpenseTrackerAPI.Models.Enums;

namespace ExpenseTrackerAPI.DTOs.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TransactionType Type { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
    }
}
