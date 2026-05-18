using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        
        [MaxLength(50)]
        public required string Username { get; set; }
        
        public required string PasswordHash { get; set; }
        
        public decimal CurrentBalance { get; set; } = 0;

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
