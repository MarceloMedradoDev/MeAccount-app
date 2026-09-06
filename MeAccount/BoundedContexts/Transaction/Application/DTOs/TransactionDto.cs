using System.ComponentModel.DataAnnotations;

namespace MeAccount.BoundedContexts.Transaction.Application.DTOs;

public record CreateTransactionDto
{
    [Required(ErrorMessage = "Conta é obrigatória")]
    public Guid AccountId { get; init; }

    public Guid? CategoryId { get; init; }

    [Required(ErrorMessage = "Tipo é obrigatório")]
    public string Type { get; init; } = string.Empty;

    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Amount { get; init; }

    [Required(ErrorMessage = "Data é obrigatória")]
    public DateTime Date { get; init; }

    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500, MinimumLength = 2, ErrorMessage = "Descrição deve ter entre 2 e 500 caracteres")]
    public string Description { get; init; } = string.Empty;

    public string? Notes { get; init; }
}

public record UpdateTransactionDto
{
    public Guid? CategoryId { get; init; }

    [Required(ErrorMessage = "Tipo é obrigatório")]
    public string Type { get; init; } = string.Empty;

    [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    public decimal Amount { get; init; }

    [Required(ErrorMessage = "Data é obrigatória")]
    public DateTime Date { get; init; }

    [Required(ErrorMessage = "Descrição é obrigatória")]
    [StringLength(500, MinimumLength = 2, ErrorMessage = "Descrição deve ter entre 2 e 500 caracteres")]
    public string Description { get; init; } = string.Empty;

    public string? Notes { get; init; }
}

public record TransactionResponseDto
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public string AccountName { get; init; } = string.Empty;
    public Guid? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public string Type { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string Description { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }
}
