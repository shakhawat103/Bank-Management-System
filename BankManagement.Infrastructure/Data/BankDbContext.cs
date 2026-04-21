// 🎯 EF Core DbContext - depends on Core for entities
// Dependency: BankManagement.Core
using BankManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Principal;

namespace BankManagement.Infrastructure.Data
{
    // 🎯 Add this constructor ONLY for design-time migrations
    // ⚠️ Don't use this in production - it's just for migrations
    public class BankDbContext : DbContext
    {
        // ✅ Existing constructor (used at runtime)
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        // 🆕 Add this for design-time tools (migrations)
        public BankDbContext() { }

        public DbSet<Account> Accounts { get; set; }

        // 🆕 Override OnConfiguring to provide options at design-time
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Only configure if not already configured (runtime DI takes precedence)
            if (!optionsBuilder.IsConfigured)
            {
                // ⚠️ Hard-coded connection string for migrations ONLY
                // In production, always use dependency injection!
                optionsBuilder.UseSqlServer(
                    "Server=DESKTOP-NK1LEK4\\SQLEXPRESS;Database=BankManagementDB;Trusted_Connection=True;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.AccountHolderName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Balance).HasPrecision(18, 2);
            });
        }
    }
}