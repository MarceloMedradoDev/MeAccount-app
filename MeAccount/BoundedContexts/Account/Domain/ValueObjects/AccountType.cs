using MeAccount.SharedKernel.Domain;

namespace MeAccount.BoundedContexts.Account.Domain.ValueObjects;

public class AccountType : ValueObject
{
    public string Value { get; private set; }

    public static readonly AccountType Checking = new("Checking");
    public static readonly AccountType Savings = new("Savings");
    public static readonly AccountType Investment = new("Investment");
    public static readonly AccountType Wallet = new("Wallet");
    public static readonly AccountType Credit = new("Credit");

    private AccountType(string value)
    {
        Value = value;
    }

    public static AccountType From(string value)
    {
        return value.ToLower() switch
        {
            "checking" => Checking,
            "savings" => Savings,
            "investment" => Investment,
            "wallet" => Wallet,
            "credit" => Credit,
            _ => throw new ArgumentException($"Invalid account type: {value}")
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
