// 🎯 Interface for calling the Web API
// Dependency: BankManagement.Application (for DTOs)
using BankManagement.Application.DTOs;

namespace BankManagement.Web.Services
{
    public interface IApiService
    {
        Task<IEnumerable<AccountDto>> GetAllAccountsAsync();
        Task<AccountDto?> GetAccountByIdAsync(int id);
        Task<AccountDto> CreateAccountAsync(CreateAccountDto createDto);
        Task<bool> UpdateAccountAsync(int id, UpdateAccountDto updateDto);
        Task<bool> DeleteAccountAsync(int id);
    }
}