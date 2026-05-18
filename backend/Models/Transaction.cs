using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        
        public decimal Amount { get; set; }
        
        public DateTime Date { get; set; }
        
        [MaxLength(500)]
        public string? Note { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
