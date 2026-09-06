using AccountEntity = MeAccount.BoundedContexts.Account.Domain.Entities.Account;

namespace MeAccount.BoundedContexts.Account.Infrastructure.Persistence;

public interface IAccountRepository
{
    Task<AccountEntity?> GetByIdAsync(Guid id, Guid userId);
    Task<IEnumerable<AccountEntity>> GetAllAsync(Guid userId);
    Task AddAsync(AccountEntity account);
    Task UpdateAsync(AccountEntity account);
    Task DeleteAsync(AccountEntity account);
}
