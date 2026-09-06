using MeAccount.SharedKernel.Domain;
using MeAccount.BoundedContexts.Transaction.Domain.ValueObjects;

namespace MeAccount.BoundedContexts.Transaction.Domain.Entities;

public class TransactionEntity : Entity
{
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid? CategoryId { get; private set; }
    public TransactionType Type { get; private set; } = null!;
    public Money Amount { get; private set; } = null!;
    public DateTime Date { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private TransactionEntity() : base() { }

    public TransactionEntity(
        Guid userId,
        Guid accountId,
        Guid? categoryId,
        TransactionType type,
        Money amount,
        DateTime date,
        string description,
        string? notes = null) : base()
    {
        UserId = userId;
        AccountId = accountId;
        CategoryId = categoryId;
        Type = type;
        Amount = amount;
        Date = date;
        Description = description;
        Notes = notes;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        Guid? categoryId,
        TransactionType type,
        Money amount,
        DateTime date,
        string description,
        string? notes = null)
    {
        CategoryId = categoryId;
        Type = type;
        Amount = amount;
        Date = date;
        Description = description;
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;
    }
}
