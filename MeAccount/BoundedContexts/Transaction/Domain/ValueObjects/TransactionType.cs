using MeAccount.SharedKernel.Domain;

namespace MeAccount.BoundedContexts.Transaction.Domain.ValueObjects;

public class TransactionType : ValueObject
{
    public string Value { get; private set; }

    public static readonly TransactionType Income = new("Income");
    public static readonly TransactionType Expense = new("Expense");
    public static readonly TransactionType Transfer = new("Transfer");

    private TransactionType(string value)
    {
        Value = value;
    }

    public static TransactionType From(string value)
    {
        return value.ToLower() switch
        {
            "income" => Income,
            "expense" => Expense,
            "transfer" => Transfer,
            _ => throw new ArgumentException($"Invalid transaction type: {value}")
        };
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value;
    }
}
