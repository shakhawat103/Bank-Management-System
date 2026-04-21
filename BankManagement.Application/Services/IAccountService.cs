// 🎯 Service interface - business logic contract
// Dependency: BankManagement.Core (for Account entity)
using BankManagement.Core.Entities;
using BankManagement.Application.DTOs;

namespace BankManagement.Application.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task<AccountDto?> GetAccountByIdAsync(int id);
        Task<AccountDto> CreateAccountAsync(CreateAccountDto createDto);
        Task<bool> UpdateAccountAsync(int id, UpdateAccountDto updateDto);
        Task<bool> DeleteAccountAsync(int id);
        Task<bool> DepositAsync(int accountId, decimal amount);
        Task<bool> WithdrawAsync(int accountId, decimal amount);
    }
}