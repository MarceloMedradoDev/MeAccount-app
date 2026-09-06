using MeAccount.BoundedContexts.Category.Application.DTOs;
using MeAccount.BoundedContexts.Category.Domain.Entities;
using MeAccount.BoundedContexts.Category.Domain.ValueObjects;
using MeAccount.BoundedContexts.Category.Infrastructure.Persistence;

namespace MeAccount.BoundedContexts.Category.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoryResponseDto> GetByIdAsync(Guid id, Guid userId)
    {
        var category = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Category not found");

        return MapToResponse(category);
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync(Guid userId, string? type = null)
    {
        var categories = await _repository.GetAllAsync(userId, type);
        return categories.Select(MapToResponse);
    }

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto, Guid userId)
    {
        if (await _repository.ExistsByNameAsync(dto.Name, userId))
            throw new InvalidOperationException("Category with this name already exists");

        var categoryType = CategoryType.From(dto.Type);
        var category = new CategoryEntity(userId, dto.Name, dto.Icon, dto.Color, categoryType);

        await _repository.AddAsync(category);

        return MapToResponse(category);
    }

    public async Task<CategoryResponseDto> UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId)
    {
        var category = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Category not found");

        if (await _repository.ExistsByNameAsync(dto.Name, userId, id))
            throw new InvalidOperationException("Category with this name already exists");

        category.Update(dto.Name, dto.Icon, dto.Color);
        await _repository.UpdateAsync(category);

        return MapToResponse(category);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var category = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Category not found");

        await _repository.DeleteAsync(category);
    }

    private static CategoryResponseDto MapToResponse(CategoryEntity category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Icon = category.Icon,
            Color = category.Color,
            Type = category.Type.Value,
            CreatedAt = category.CreatedAt
        };
    }
}
