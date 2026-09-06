using MeAccount.BoundedContexts.Transaction.Application.DTOs;

namespace MeAccount.BoundedContexts.Transaction.Application.Services;

public interface ITransactionService
{
    Task<TransactionResponseDto> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<TransactionResponseDto>> GetAllAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null, Guid? accountId = null, Guid? categoryId = null, string? type = null);
    Task<TransactionResponseDto> CreateAsync(CreateTransactionDto dto, Guid userId);
    Task<TransactionResponseDto> UpdateAsync(Guid id, UpdateTransactionDto dto, Guid userId);
    Task DeleteAsync(Guid id, Guid userId);
}
