namespace ExpenseTrackerAPI.DTOs.Dashboard
{
    public class DashboardSummaryDto
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetIncome => TotalIncome - TotalExpense;
        public IEnumerable<CategoryBreakdownDto> IncomeBreakdown { get; set; } = new List<CategoryBreakdownDto>();
        public IEnumerable<CategoryBreakdownDto> ExpenseBreakdown { get; set; } = new List<CategoryBreakdownDto>();
    }

    public class CategoryBreakdownDto
    {
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryColor { get; set; }
        public decimal TotalAmount { get; set; }
        public double Percentage { get; set; }
    }
}
