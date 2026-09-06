using System.ComponentModel.DataAnnotations;

namespace MeAccount.BoundedContexts.Category.Application.DTOs;

public record CreateCategoryDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Ícone é obrigatório")]
    public string Icon { get; init; } = string.Empty;

    [Required(ErrorMessage = "Cor é obrigatória")]
    public string Color { get; init; } = string.Empty;

    [Required(ErrorMessage = "Tipo é obrigatório")]
    public string Type { get; init; } = string.Empty;
}

public record UpdateCategoryDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 100 caracteres")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Ícone é obrigatório")]
    public string Icon { get; init; } = string.Empty;

    [Required(ErrorMessage = "Cor é obrigatória")]
    public string Color { get; init; } = string.Empty;
}

public record CategoryResponseDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
    public string Color { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
