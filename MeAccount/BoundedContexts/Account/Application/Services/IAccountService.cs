using MeAccount.BoundedContexts.Account.Application.DTOs;

namespace MeAccount.BoundedContexts.Account.Application.Services;

public interface IAccountService
{
    Task<AccountResponseDto> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<AccountResponseDto>> GetAllAsync(Guid userId);
    Task<AccountResponseDto> CreateAsync(CreateAccountDto dto, Guid userId);
    Task<AccountResponseDto> UpdateAsync(Guid id, UpdateAccountDto dto, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
}
