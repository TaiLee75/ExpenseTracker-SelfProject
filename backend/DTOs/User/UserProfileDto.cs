namespace ExpenseTrackerAPI.DTOs.User
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public decimal CurrentBalance { get; set; }
    }
}
