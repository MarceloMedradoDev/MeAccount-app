using MeAccount.BoundedContexts.Category.Domain.Entities;

namespace MeAccount.BoundedContexts.Category.Infrastructure.Persistence;

public interface ICategoryRepository
{
    Task<CategoryEntity?> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<CategoryEntity>> GetAllAsync(Guid userId, string? type = null);
    Task AddAsync(CategoryEntity category);
    Task UpdateAsync(CategoryEntity category);
    Task DeleteAsync(CategoryEntity category);
    Task<bool> ExistsByNameAsync(string name, Guid userId, Guid? excludeId = null);
}
