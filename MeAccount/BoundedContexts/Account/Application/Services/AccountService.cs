using MeAccount.BoundedContexts.Account.Application.DTOs;
using MeAccount.BoundedContexts.Account.Domain.ValueObjects;
using MeAccount.BoundedContexts.Account.Infrastructure.Persistence;
using AccountEntity = MeAccount.BoundedContexts.Account.Domain.Entities.Account;

namespace MeAccount.BoundedContexts.Account.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;

    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }

    public async Task<AccountResponseDto> GetByIdAsync(Guid id, Guid userId)
    {
        var account = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Account not found");

        return MapToResponse(account);
    }

    public async Task<IEnumerable<AccountResponseDto>> GetAllAsync(Guid userId)
    {
        var accounts = await _repository.GetAllAsync(userId);
        return accounts.Select(MapToResponse);
    }

    public async Task<AccountResponseDto> CreateAsync(CreateAccountDto dto, Guid userId)
    {
        var account = new AccountEntity(userId, dto.Name, AccountType.From(dto.Type), dto.InitialBalance);

        await _repository.AddAsync(account);

        return MapToResponse(account);
    }

    public async Task<AccountResponseDto> UpdateAsync(Guid id, UpdateAccountDto dto, Guid userId)
    {
        var account = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Account not found");

        account.Update(dto.Name, AccountType.From(dto.Type));
        await _repository.UpdateAsync(account);

        return MapToResponse(account);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var account = await _repository.GetByIdAsync(id, userId)
            ?? throw new KeyNotFoundException("Account not found");

        await _repository.DeleteAsync(account);
    }

    private static AccountResponseDto MapToResponse(AccountEntity account)
    {
        return new AccountResponseDto
        {
            Id = account.Id,
            Name = account.Name,
            Type = account.Type.ToString(),
            Balance = account.Balance,
            Currency = account.Currency
        };
    }
}
