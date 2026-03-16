using System.ComponentModel.DataAnnotations;

namespace BoundedContextCanvas.Api.Contracts.Requests;

/// <summary>Добавление бизнес-решения в контекст.</summary>
public sealed record AddBusinessDecisionRequest
{
    /// <summary>Формулировка бизнес-правила.</summary>
    [Required] public string Rule { get; init; } = default!;
    /// <summary>Обоснование принятого решения.</summary>
    [Required] public string Rationale { get; init; } = default!;
}
