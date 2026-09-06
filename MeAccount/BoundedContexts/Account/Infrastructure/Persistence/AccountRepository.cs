using Microsoft.EntityFrameworkCore;
using MeAccount.Data;
using AccountEntity = MeAccount.BoundedContexts.Account.Domain.Entities.Account;

namespace MeAccount.BoundedContexts.Account.Infrastructure.Persistence;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AccountEntity?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
    }

    public async Task<IEnumerable<AccountEntity>> GetAllAsync(Guid userId)
    {
        return await _context.Accounts
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.Name)
            .ToListAsync();
    }

    public async Task AddAsync(AccountEntity account)
    {
        await _context.Accounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AccountEntity account)
    {
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AccountEntity account)
    {
        _context.Accounts.Remove(account);
        await _context.SaveChangesAsync();
    }
}
