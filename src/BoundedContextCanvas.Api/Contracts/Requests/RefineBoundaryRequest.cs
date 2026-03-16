using System.ComponentModel.DataAnnotations;

namespace BoundedContextCanvas.Api.Contracts.Requests;

/// <summary>Определение границ контекста через ответственности.</summary>
public sealed record RefineBoundaryRequest
{
    /// <summary>Список ответственностей контекста.</summary>
    [Required] public List<ResponsibilityItem> Responsibilities { get; init; } = new();
}

public sealed record ResponsibilityItem
{
    /// <summary>Описание ответственности.</summary>
    [Required] public string Description { get; init; } = default!;
    /// <summary>Does | DoesNot</summary>
    [Required] public string Type { get; init; } = default!;
}
