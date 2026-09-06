using MeAccount.SharedKernel.Domain;
using MeAccount.BoundedContexts.Account.Domain.ValueObjects;

namespace MeAccount.BoundedContexts.Account.Domain.Entities;

public class Account : Entity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public AccountType Type { get; private set; } = null!;
    public decimal Balance { get; private set; }
    public string Currency { get; private set; } = "BRL";
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Account() : base() { }

    public Account(
        Guid userId,
        string name,
        AccountType type,
        decimal initialBalance = 0) : base()
    {
        UserId = userId;
        Name = name;
        Type = type;
        Balance = initialBalance;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, AccountType type)
    {
        Name = name;
        Type = type;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateBalance(decimal amount)
    {
        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetBalance(decimal balance)
    {
        Balance = balance;
        UpdatedAt = DateTime.UtcNow;
    }
}
