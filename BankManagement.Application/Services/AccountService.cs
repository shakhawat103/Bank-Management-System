// 🎯 Service implementation - business logic
// Dependencies: BankManagement.Core, BankManagement.Application
using AutoMapper;
using BankManagement.Core.Interfaces;
using BankManagement.Core.Entities;
using BankManagement.Application.DTOs;

namespace BankManagement.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAccountsAsync()
        {
            var accounts = await _unitOfWork.Repository<Account>().GetAllAsync();
            return _mapper.Map<IEnumerable<AccountDto>>(accounts);
        }

        public async Task<AccountDto?> GetAccountByIdAsync(int id)
        {
            var account = await _unitOfWork.Repository<Account>().GetByIdAsync(id);
            return account != null ? _mapper.Map<AccountDto>(account) : null;
        }

        public async Task<AccountDto> CreateAccountAsync(CreateAccountDto createDto)
        {
            var account = _mapper.Map<Account>(createDto);
            await _unitOfWork.Repository<Account>().AddAsync(account);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<AccountDto>(account);
        }

        public async Task<bool> UpdateAccountAsync(int id, UpdateAccountDto updateDto)
        {
            var account = await _unitOfWork.Repository<Account>().GetByIdAsync(id);
            if (account == null) return false;

            _mapper.Map(updateDto, account);
            _unitOfWork.Repository<Account>().Update(account);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var account = await _unitOfWork.Repository<Account>().GetByIdAsync(id);
            if (account == null) return false;

            _unitOfWork.Repository<Account>().Delete(account);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DepositAsync(int accountId, decimal amount)
        {
            if (amount <= 0) return false;

            var account = await _unitOfWork.Repository<Account>().GetByIdAsync(accountId);
            if (account == null || !account.IsActive) return false;

            account.Balance += amount;
            _unitOfWork.Repository<Account>().Update(account);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> WithdrawAsync(int accountId, decimal amount)
        {
            if (amount <= 0) return false;

            var account = await _unitOfWork.Repository<Account>().GetByIdAsync(accountId);
            if (account == null || !account.IsActive || account.Balance < amount) return false;

            account.Balance -= amount;
            _unitOfWork.Repository<Account>().Update(account);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}