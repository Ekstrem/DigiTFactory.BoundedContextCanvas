using System.ComponentModel.DataAnnotations;

namespace BoundedContextCanvas.Api.Contracts.Requests;

/// <summary>Создание нового Bounded Context Canvas.</summary>
public sealed record CreateBoundedContextRequest
{
    /// <summary>Техническое имя (PascalCase, ^[A-Z][a-zA-Z0-9]*$).</summary>
    [Required] public string TechnicalName { get; init; } = default!;
    /// <summary>Команда-владелец контекста.</summary>
    [Required] public string OwnerTeam { get; init; } = default!;
    /// <summary>Бизнес-назначение контекста (1–2 предложения).</summary>
    [Required] public string BusinessPurpose { get; init; } = default!;
}
