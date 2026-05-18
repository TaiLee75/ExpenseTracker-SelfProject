using ExpenseTrackerAPI.DTOs.User;
using ExpenseTrackerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Bắt buộc phải có Token JWT
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdStr!);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _userService.GetUserProfileAsync(GetUserId());
            return Ok(profile);
        }

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var userId = GetUserId();
            var balance = await _userService.GetCurrentBalanceAsync(userId);
            return Ok(new { CurrentBalance = balance });
        }

        [HttpPut("balance")]
        public async Task<IActionResult> UpdateBalance([FromBody] UpdateBalanceDto dto)
        {
            var userId = GetUserId();
            var success = await _userService.UpdateBalanceAsync(userId, dto.NewBalance);
            if (!success)
            {
                return BadRequest(new { Message = "Cập nhật số dư thất bại." });
            }
            return Ok(new { Message = "Cập nhật số dư thành công!", NewBalance = dto.NewBalance });
        }
    }
}
