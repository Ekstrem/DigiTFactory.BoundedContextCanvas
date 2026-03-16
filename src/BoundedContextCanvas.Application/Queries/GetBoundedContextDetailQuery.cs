using MediatR;

namespace BoundedContextCanvas.Application.Queries;

public sealed record GetBoundedContextDetailQuery(Guid Id) : IRequest<BoundedContextDetailResult?>;

public sealed record BoundedContextDetailResult(
    Guid Id,
    string TechnicalName,
    string OwnerTeam,
    string Status,
    string? BusinessPurpose,
    StrategicClassificationResult? StrategicClassification,
    DomainRoleResult? DomainRole,
    IReadOnlyList<ResponsibilityResult> Responsibilities,
    IReadOnlyList<LanguageTermResult> Language,
    IReadOnlyList<RelationshipResult> Relationships,
    IReadOnlyList<PublicInterfaceItemResult> PublicInterface,
    IReadOnlyList<BusinessDecisionResult> BusinessDecisions,
    IReadOnlyList<AssumptionResult> Assumptions,
    long Version,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public sealed record StrategicClassificationResult(string DomainType, string BusinessModelRole, string Evolution);
public sealed record DomainRoleResult(string RoleType, string Description);
public sealed record ResponsibilityResult(string Description, string Type);
public sealed record LanguageTermResult(string Term, string Definition);
public sealed record RelationshipResult(Guid TargetContextId, string TargetContextName, string Direction, string Pattern);
public sealed record PublicInterfaceItemResult(string Name, string Type, string Direction, string LinkedResponsibility);
public sealed record BusinessDecisionResult(string Rule, string Rationale);
public sealed record AssumptionResult(string Statement, string Risk);
