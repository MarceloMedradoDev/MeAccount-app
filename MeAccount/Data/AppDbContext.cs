using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MeAccount.Models;
using MeAccount.BoundedContexts.Category.Domain.Entities;
using MeAccount.BoundedContexts.Category.Domain.ValueObjects;
using MeAccount.BoundedContexts.Transaction.Domain.Entities;
using MeAccount.BoundedContexts.Transaction.Domain.ValueObjects;
using MeAccount.BoundedContexts.Account.Domain.Entities;
using MeAccount.BoundedContexts.Account.Domain.ValueObjects;

namespace MeAccount.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<Account> Accounts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Category configuration
        builder.Entity<CategoryEntity>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Icon).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Color).IsRequired().HasMaxLength(7);
            
            // Configure CategoryType value object
            var categoryTypeConverter = new ValueConverter<CategoryType, string>(
                v => v.Value,
                v => CategoryType.From(v));
            
            entity.Property(c => c.Type)
                .HasConversion(categoryTypeConverter)
                .HasMaxLength(20);

            entity.Property(c => c.UserId).IsRequired();
        });

        // Transaction configuration
        builder.Entity<TransactionEntity>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Description).IsRequired().HasMaxLength(500);
            entity.Property(t => t.Notes).HasMaxLength(2000);
            
            // Configure TransactionType value object
            var transactionTypeConverter = new ValueConverter<TransactionType, string>(
                v => v.Value,
                v => TransactionType.From(v));
            
            entity.Property(t => t.Type)
                .HasConversion(transactionTypeConverter)
                .HasMaxLength(20);
            
            // Configure Money value object
            var moneyConverter = new ValueConverter<Money, decimal>(
                v => v.Amount,
                v => new Money(v));
            
            entity.Property(t => t.Amount)
                .HasConversion(moneyConverter)
                .HasColumnType("decimal(18,2)");

            entity.Property(t => t.UserId).IsRequired();
        });

        // Account configuration
        builder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Name).IsRequired().HasMaxLength(100);

            var accountTypeConverter = new ValueConverter<AccountType, string>(
                v => v.Value,
                v => AccountType.From(v));

            entity.Property(a => a.Type)
                .HasConversion(accountTypeConverter)
                .HasMaxLength(20);

            entity.Property(a => a.Balance).HasColumnType("decimal(18,2)");
            entity.Property(a => a.UserId).IsRequired();
        });
    }
}
