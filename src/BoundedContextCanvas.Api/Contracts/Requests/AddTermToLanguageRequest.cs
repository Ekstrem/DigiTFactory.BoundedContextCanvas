using System.ComponentModel.DataAnnotations;

namespace BoundedContextCanvas.Api.Contracts.Requests;

/// <summary>Добавление термина в Ubiquitous Language контекста.</summary>
public sealed record AddTermToLanguageRequest
{
    /// <summary>Термин на языке домена.</summary>
    [Required] public string Term { get; init; } = default!;
    /// <summary>Определение термина в рамках данного контекста.</summary>
    [Required] public string Definition { get; init; } = default!;
}
