using System.ComponentModel.DataAnnotations;
using ExpenseTrackerAPI.Models.Enums;

namespace ExpenseTrackerAPI.DTOs.Category
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loại giao dịch (Thu/Chi) là bắt buộc")]
        public TransactionType Type { get; set; }

        [MaxLength(50)]
        public string? Icon { get; set; }

        [MaxLength(20)]
        public string? Color { get; set; }
    }
}
