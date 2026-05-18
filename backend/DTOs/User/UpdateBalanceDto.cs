using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.DTOs.User
{
    public class UpdateBalanceDto
    {
        [Required(ErrorMessage = "Vui lòng nhập số dư mới")]
        public decimal NewBalance { get; set; }
    }
}
