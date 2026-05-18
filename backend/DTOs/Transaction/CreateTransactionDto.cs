using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.Transaction
{
    public class CreateTransactionDto
    {
        [Required(ErrorMessage = "Số tiền là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0")]
        public decimal Amount { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [MaxLength(500)]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        public Guid CategoryId { get; set; }
    }
}
