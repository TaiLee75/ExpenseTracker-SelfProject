using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;
    }
}
