using MeAccount.SharedKernel.Domain;

namespace MeAccount.BoundedContexts.Category.Domain.ValueObjects;

public class CategoryType : ValueObject
{
    public string Value { get; private set; }

    public static readonly CategoryType Income = new("Income");
    public static readonly CategoryType Expense = new("Expense");

    private CategoryType(string value)
    {
        Value = value;
    }

    public static CategoryType From(string value)
    {
        return value.ToLower() switch
        {
            "income" => Income,
            "expense" => Expense,
            _ => throw new ArgumentException($"Invalid category type: {value}")
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
