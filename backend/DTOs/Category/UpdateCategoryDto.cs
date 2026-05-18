using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? Icon { get; set; }

        [MaxLength(20)]
        public string? Color { get; set; }
    }
}
