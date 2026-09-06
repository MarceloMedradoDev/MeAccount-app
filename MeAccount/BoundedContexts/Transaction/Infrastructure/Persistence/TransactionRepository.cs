using Microsoft.EntityFrameworkCore;
using MeAccount.Data;
using MeAccount.BoundedContexts.Transaction.Domain.Entities;
using MeAccount.BoundedContexts.Transaction.Domain.ValueObjects;

namespace MeAccount.BoundedContexts.Transaction.Infrastructure.Persistence;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TransactionEntity?> GetByIdAsync(Guid id, Guid userId)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<IEnumerable<TransactionEntity>> GetAllAsync(
        Guid userId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        Guid? accountId = null,
        Guid? categoryId = null,
        string? type = null)
    {
        var query = _context.Transactions
            .Where(t => t.UserId == userId);

        if (startDate.HasValue)
            query = query.Where(t => t.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(t => t.Date < endDate.Value.AddDays(1));

        if (accountId.HasValue)
            query = query.Where(t => t.AccountId == accountId.Value);

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (!string.IsNullOrEmpty(type))
        {
            var transactionType = TransactionType.From(type);
            query = query.Where(t => t.Type == transactionType);
        }

        return await query
            .OrderByDescending(t => t.Date)
            .ToListAsync();
    }

    public async Task AddAsync(TransactionEntity transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TransactionEntity transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TransactionEntity transaction)
    {
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
    }
}
