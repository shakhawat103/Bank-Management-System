// 🎯 Domain Entity - NO dependencies on other projects
namespace BankManagement.Core.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountHolderName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string AccountType { get; set; } = "Savings"; // Savings/Current
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}