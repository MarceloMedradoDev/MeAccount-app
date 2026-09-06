using MeAccount.BoundedContexts.Transaction.Application.DTOs;
using MeAccount.BoundedContexts.Transaction.Domain.Entities;
using MeAccount.BoundedContexts.Transaction.Domain.ValueObjects;
using MeAccount.BoundedContexts.Transaction.Infrastructure.Persistence;
using MeAccount.BoundedContexts.Account.Infrastructure.Persistence;
using MeAccount.BoundedContexts.Category.Infrastructure.Persistence;
using MeAccount.Data;
using AccountEntity = MeAccount.BoundedContexts.Account.Domain.Entities.Account;

namespace MeAccount.BoundedContexts.Transaction.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly IAccountRepository _accountRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly AppDbContext _context;

    public TransactionService(
        ITransactionRepository repository,
        IAccountRepository accountRepository,
        ICategoryRepository categoryRepository,
        AppDbContext context)
    {
        _repository = repository;
        _accountRepository = accountRepository;
        _categoryRepository = categoryRepository;
        _context = context;
    }

    public async Task<TransactionResponseDto> GetByIdAsync(Guid id, Guid userId)
    {
        var transaction = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Transaction not found");

        return await MapToResponse(transaction, userId);
    }

    public async Task<IEnumerable<TransactionResponseDto>> GetAllAsync(
        Guid userId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        Guid? accountId = null,
        Guid? categoryId = null,
        string? type = null)
    {
        var transactions = await _repository.GetAllAsync(userId, startDate, endDate, accountId, categoryId, type);
        var result = new List<TransactionResponseDto>();

        foreach (var transaction in transactions)
        {
            result.Add(await MapToResponse(transaction, userId));
        }

        return result;
    }

    public async Task<TransactionResponseDto> CreateAsync(CreateTransactionDto dto, Guid userId)
    {
        var transactionType = TransactionType.From(dto.Type);
        var money = new Money(dto.Amount);

        await using var dbTransaction = await _context.Database.BeginTransactionAsync();

        var account = await _accountRepository.GetByIdAsync(dto.AccountId, userId)
            ?? throw new KeyNotFoundException("Account not found");

        if (dto.CategoryId.HasValue)
        {
            _ = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value, userId)
                ?? throw new KeyNotFoundException("Category not found");
        }

        var transaction = new TransactionEntity(
            userId,
            dto.AccountId,
            dto.CategoryId,
            transactionType,
            money,
            dto.Date,
            dto.Description,
            dto.Notes);

        await _repository.AddAsync(transaction);

        ApplyBalanceChange(account, transactionType, money);
        await _accountRepository.UpdateAsync(account);

        await dbTransaction.CommitAsync();

        return await MapToResponse(transaction, userId);
    }

    public async Task<TransactionResponseDto> UpdateAsync(Guid id, UpdateTransactionDto dto, Guid userId)
    {
        var existingTransaction = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Transaction not found");

        var transactionType = TransactionType.From(dto.Type);
        var money = new Money(dto.Amount);

        await using var dbTransaction = await _context.Database.BeginTransactionAsync();

        if (dto.CategoryId.HasValue)
        {
            _ = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value, userId)
                ?? throw new KeyNotFoundException("Category not found");
        }

        // Reverse old balance effect
        await UpdateAccountBalanceAsync(existingTransaction.AccountId, userId, existingTransaction.Type, existingTransaction.Amount, isReversal: true);

        // Update transaction
        existingTransaction.Update(
            dto.CategoryId,
            transactionType,
            money,
            dto.Date,
            dto.Description,
            dto.Notes);

        await _repository.UpdateAsync(existingTransaction);

        // Apply new balance effect
        await UpdateAccountBalanceAsync(existingTransaction.AccountId, userId, transactionType, money);

        await dbTransaction.CommitAsync();

        return await MapToResponse(existingTransaction, userId);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var transaction = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Transaction not found");

        await using var dbTransaction = await _context.Database.BeginTransactionAsync();

        // Reverse balance effect
        await UpdateAccountBalanceAsync(transaction.AccountId, userId, transaction.Type, transaction.Amount, isReversal: true);

        await _repository.DeleteAsync(transaction);

        await dbTransaction.CommitAsync();
    }

    private async Task UpdateAccountBalanceAsync(Guid accountId, Guid userId, TransactionType type, Money amount, bool isReversal = false)
    {
        var account = await _accountRepository.GetByIdAsync(accountId, userId)
            ?? throw new KeyNotFoundException("Account not found");

        ApplyBalanceChange(account, type, amount, isReversal);
        await _accountRepository.UpdateAsync(account);
    }

    private static void ApplyBalanceChange(AccountEntity account, TransactionType type, Money amount, bool isReversal = false)
    {
        var multiplier = isReversal ? -1 : 1;

        // TODO: Suporte a Transferência com duas contas (ToAccountId) futuro
        var direction = type == TransactionType.Transfer
            ? 0
            : type == TransactionType.Income ? 1 : -1;

        account.UpdateBalance(amount.Amount * direction * multiplier);
    }

    private async Task<TransactionResponseDto> MapToResponse(TransactionEntity transaction, Guid userId)
    {
        var account = await _accountRepository.GetByIdAsync(transaction.AccountId, userId);
        var category = transaction.CategoryId.HasValue
            ? await _categoryRepository.GetByIdAsync(transaction.CategoryId.Value, userId)
            : null;

        return new TransactionResponseDto
        {
            Id = transaction.Id,
            AccountId = transaction.AccountId,
            AccountName = account?.Name ?? "Unknown",
            CategoryId = transaction.CategoryId,
            CategoryName = category?.Name,
            Type = transaction.Type.Value,
            Amount = transaction.Amount.Amount,
            Currency = transaction.Amount.Currency,
            Date = transaction.Date,
            Description = transaction.Description,
            Notes = transaction.Notes,
            CreatedAt = transaction.CreatedAt
        };
    }
}
