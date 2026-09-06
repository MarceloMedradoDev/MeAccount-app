using Microsoft.EntityFrameworkCore;
using MeAccount.Data;
using MeAccount.BoundedContexts.Category.Domain.Entities;
using MeAccount.BoundedContexts.Category.Domain.ValueObjects;

namespace MeAccount.BoundedContexts.Category.Infrastructure.Persistence;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CategoryEntity?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }

    public async Task<IEnumerable<CategoryEntity>> GetAllAsync(Guid userId, string? type = null)
    {
        var query = _context.Categories
            .Where(c => c.UserId == userId);

        if (!string.IsNullOrEmpty(type))
        {
            var categoryType = CategoryType.From(type);
            query = query.Where(c => c.Type == categoryType);
        }

        return await query
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task AddAsync(CategoryEntity category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CategoryEntity category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(CategoryEntity category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid userId, Guid? excludeId = null)
    {
        return await _context.Categories
            .AnyAsync(c => c.Name == name && c.UserId == userId && c.Id != excludeId);
    }
}
