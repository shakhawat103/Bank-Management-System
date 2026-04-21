// 🎯 Data Transfer Object - what API sends/receives
// Dependency: None (pure POCO)
namespace BankManagement.Application.DTOs
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolderName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }

    // For creating new account
    public class CreateAccountDto
    {
        public string AccountHolderName { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
        public string AccountType { get; set; } = "Savings";
    }

    // For updating account
    public class UpdateAccountDto
    {
        public string AccountHolderName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
    }
}