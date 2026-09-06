using MeAccount.SharedKernel.Domain;
using MeAccount.BoundedContexts.Category.Domain.ValueObjects;

namespace MeAccount.BoundedContexts.Category.Domain.Entities;

public class CategoryEntity : Entity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Icon { get; private set; } = string.Empty;
    public string Color { get; private set; } = string.Empty;
    public CategoryType Type { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private CategoryEntity() : base() { }

    public CategoryEntity(
        Guid userId,
        string name,
        string icon,
        string color,
        CategoryType type) : base()
    {
        UserId = userId;
        Name = name;
        Icon = icon;
        Color = color;
        Type = type;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(
        string name,
        string icon,
        string color)
    {
        Name = name;
        Icon = icon;
        Color = color;
        UpdatedAt = DateTime.UtcNow;
    }
}
