using System.ComponentModel.DataAnnotations;

namespace BoundedContextCanvas.Api.Contracts.Requests;

/// <summary>Установка связи с другим ограниченным контекстом.</summary>
public sealed record MapRelationshipRequest
{
    /// <summary>Идентификатор целевого контекста.</summary>
    [Required] public Guid TargetContextId { get; init; }
    /// <summary>Имя целевого контекста.</summary>
    [Required] public string TargetContextName { get; init; } = default!;
    /// <summary>Upstream | Downstream | Symmetric</summary>
    [Required] public string Direction { get; init; } = default!;
    /// <summary>SharedKernel | CustomerSupplier | Conformist | AnticorruptionLayer | OpenHostService | PublishedLanguage | SeparateWays | Partnership</summary>
    [Required] public string Pattern { get; init; } = default!;
}
