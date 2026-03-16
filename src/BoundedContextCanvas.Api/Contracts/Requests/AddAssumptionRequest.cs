using System.ComponentModel.DataAnnotations;

namespace BoundedContextCanvas.Api.Contracts.Requests;

/// <summary>Добавление предположения (assumption) в контекст.</summary>
public sealed record AddAssumptionRequest
{
    /// <summary>Формулировка предположения.</summary>
    [Required] public string Statement { get; init; } = default!;
    /// <summary>Связанный риск при невыполнении предположения.</summary>
    [Required] public string Risk { get; init; } = default!;
}
