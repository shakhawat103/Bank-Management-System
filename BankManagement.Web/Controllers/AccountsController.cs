using Microsoft.AspNetCore.Mvc;
using BankManagement.Application.DTOs;
using BankManagement.Web.Services;

namespace BankManagement.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IApiService _apiService;

        public AccountsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _apiService.GetAllAccountsAsync();
            return View(accounts);
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var account = await _apiService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAccountDto createDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.CreateAccountAsync(createDto);
                return RedirectToAction(nameof(Index));
            }
            return View(createDto);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _apiService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();

            var updateDto = new UpdateAccountDto
            {
                AccountHolderName = account.AccountHolderName,
                Balance = account.Balance,
                IsActive = account.IsActive
            };

            ViewData["AccountId"] = id;
            return View(updateDto);
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateAccountDto updateDto)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateAccountAsync(id, updateDto);
                return RedirectToAction(nameof(Details), new { id = id });
            }
            ViewData["AccountId"] = id;
            return View(updateDto);
        }

        // ✅ GET: Accounts/Delete/5 - Shows confirmation page
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _apiService.GetAccountByIdAsync(id);
            if (account == null) return NotFound();
            return View(account);
        }

        // ✅ POST: Accounts/DeleteConfirmed/5 - Actually deletes
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // First check if account has balance
                var account = await _apiService.GetAccountByIdAsync(id);
                if (account == null) return NotFound();

                if (account.Balance != 0)
                {
                    TempData["ErrorMessage"] = "Cannot delete account with non-zero balance. Please withdraw the balance first.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }

                await _apiService.DeleteAccountAsync(id);
                TempData["SuccessMessage"] = "Account deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting account: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { id = id });
            }
        }
    }
}