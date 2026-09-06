using MeAccount.BoundedContexts.Category.Application.DTOs;

namespace MeAccount.BoundedContexts.Category.Application.Services;

public interface ICategoryService
{
    Task<CategoryResponseDto> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync(Guid userId, string? type = null);
    Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto, Guid userId);
    Task<CategoryResponseDto> UpdateAsync(Guid id, UpdateCategoryDto dto, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
}
