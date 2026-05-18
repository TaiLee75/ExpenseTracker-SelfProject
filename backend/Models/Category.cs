using System.ComponentModel.DataAnnotations;
using ExpenseTrackerAPI.Models.Enums;

namespace ExpenseTrackerAPI.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        
        [MaxLength(100)]
        public required string Name { get; set; }
        
        public TransactionType Type { get; set; }
        
        [MaxLength(50)]
        public string? Icon { get; set; }
        
        [MaxLength(20)]
        public string? Color { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
