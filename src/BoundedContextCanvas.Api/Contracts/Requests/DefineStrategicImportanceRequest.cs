using System.ComponentModel.DataAnnotations;

namespace BoundedContextCanvas.Api.Contracts.Requests;

/// <summary>Стратегическая классификация и роль домена.</summary>
public sealed record DefineStrategicImportanceRequest
{
    /// <summary>Core | Supporting | Generic</summary>
    [Required] public string DomainType { get; init; } = default!;
    /// <summary>RevenueGenerator | EngagementCreator | ComplianceEnforcer</summary>
    [Required] public string BusinessModelRole { get; init; } = default!;
    /// <summary>Genesis | CustomBuilt | Product | Commodity</summary>
    [Required] public string Evolution { get; init; } = default!;
    /// <summary>Execution | Gateway | Analysis</summary>
    [Required] public string RoleType { get; init; } = default!;
    /// <summary>Пояснение роли домена.</summary>
    [Required] public string RoleDescription { get; init; } = default!;
}
