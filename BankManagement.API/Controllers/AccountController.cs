// 🎯 API Controller - returns JSON
// Dependencies: BankManagement.Application (services + DTOs)
using Microsoft.AspNetCore.Mvc;
using BankManagement.Application.Services;
using BankManagement.Application.DTOs;

namespace BankManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        // 🔗 Dependency injected via constructor
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts); // Returns JSON
        }

        // GET: api/accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetById(int id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        // POST: api/accounts
        [HttpPost]
        public async Task<ActionResult<AccountDto>> Create(CreateAccountDto createDto)
        {
            var newAccount = await _accountService.CreateAccountAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = newAccount.Id }, newAccount);
        }

        // PUT: api/accounts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateAccountDto updateDto)
        {
            var result = await _accountService.UpdateAccountAsync(id, updateDto);
            if (!result) return NotFound();
            return NoContent();
        }

        // DELETE: api/accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accountService.DeleteAccountAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        // POST: api/accounts/5/deposit
        [HttpPost("{id}/deposit")]
        public async Task<IActionResult> Deposit(int id, [FromBody] decimal amount)
        {
            var result = await _accountService.DepositAsync(id, amount);
            if (!result) return BadRequest("Deposit failed");
            return Ok(new { message = "Deposit successful" });
        }

        // POST: api/accounts/5/withdraw
        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> Withdraw(int id, [FromBody] decimal amount)
        {
            var result = await _accountService.WithdrawAsync(id, amount);
            if (!result) return BadRequest("Withdrawal failed - insufficient funds");
            return Ok(new { message = "Withdrawal successful" });
        }
    }
}