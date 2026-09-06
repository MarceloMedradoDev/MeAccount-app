using MeAccount.BoundedContexts.Transaction.Domain.Entities;

namespace MeAccount.BoundedContexts.Transaction.Infrastructure.Persistence;

public interface ITransactionRepository
{
    Task<TransactionEntity?> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<TransactionEntity>> GetAllAsync(Guid userId, DateTime? startDate = null, DateTime? endDate = null, Guid? accountId = null, Guid? categoryId = null, string? type = null);
    Task AddAsync(TransactionEntity transaction);
    Task UpdateAsync(TransactionEntity transaction);
    Task DeleteAsync(TransactionEntity transaction);
}
