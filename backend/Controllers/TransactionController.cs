using ExpenseTrackerAPI.DTOs.Transaction;
using ExpenseTrackerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdStr!);
        }

        [HttpGet]
        public async Task<IActionResult> GetByMonth([FromQuery] int year, [FromQuery] int month)
        {
            if (year == 0) year = DateTime.Now.Year;
            if (month == 0) month = DateTime.Now.Month;

            var transactions = await _transactionService.GetUserTransactionsByMonthAsync(GetUserId(), year, month);
            return Ok(transactions);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecent([FromQuery] int count = 5)
        {
            var transactions = await _transactionService.GetRecentTransactionsAsync(GetUserId(), count);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id, GetUserId());
            if (transaction == null) return NotFound(new { Message = "Không tìm thấy giao dịch." });
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTransactionDto dto)
        {
            var transaction = await _transactionService.CreateTransactionAsync(GetUserId(), dto);
            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTransactionDto dto)
        {
            var updatedTransaction = await _transactionService.UpdateTransactionAsync(id, GetUserId(), dto);
            if (updatedTransaction == null) return NotFound(new { Message = "Không tìm thấy giao dịch." });
            return Ok(updatedTransaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _transactionService.DeleteTransactionAsync(id, GetUserId());
            if (!success) return NotFound(new { Message = "Không tìm thấy giao dịch." });
            return Ok(new { Message = "Đã xóa giao dịch và hoàn tác số dư thành công." });
        }
    }
}
