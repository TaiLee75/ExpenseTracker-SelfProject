using ExpenseTrackerAPI.Data;
using ExpenseTrackerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Test/check-db
        [HttpGet("check-db")]
        public async Task<IActionResult> CheckDbConnection()
        {
            try
            {
                // Thử đếm số lượng User để xác nhận EF Core chạy được
                var userCount = await _context.Users.CountAsync();
                return Ok(new 
                { 
                    Message = "Kết nối Database thành công rực rỡ!", 
                    DatabaseName = _context.Database.GetDbConnection().Database,
                    TotalUsersInDb = userCount 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Lỗi kết nối Database", Error = ex.Message });
            }
        }
        
        // POST: api/Test/add-dummy-user
        [HttpPost("add-dummy-user")]
        public async Task<IActionResult> AddDummyUser()
        {
            var user = new User
            {
                Username = "testuser_" + Guid.NewGuid().ToString().Substring(0, 5),
                PasswordHash = "dummyhash123",
                CurrentBalance = 5000000 // 5 triệu
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Đã thêm 1 user thành công vào Database", User = user });
        }
    }
}
