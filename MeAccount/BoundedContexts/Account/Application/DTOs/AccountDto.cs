using System.ComponentModel.DataAnnotations;

namespace MeAccount.BoundedContexts.Account.Application.DTOs;

public record CreateAccountDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Tipo é obrigatório")]
    public string Type { get; init; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Saldo inicial deve ser positivo")]
    public decimal InitialBalance { get; init; }
}

public record UpdateAccountDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Tipo é obrigatório")]
    public string Type { get; init; } = string.Empty;
}
