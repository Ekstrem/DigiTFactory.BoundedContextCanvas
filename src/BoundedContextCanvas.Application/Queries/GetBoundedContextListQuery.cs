using MediatR;

namespace BoundedContextCanvas.Application.Queries;

public sealed record GetBoundedContextListQuery(
    string? OwnerTeam = null,
    string? DomainType = null,
    string? Status = null,
    string? Search = null,
    int Page = 1,
    int PageSize = 20,
    string SortBy = "updatedAt",
    string SortDir = "desc") : IRequest<BoundedContextListResult>;

public sealed record BoundedContextListResult(
    IReadOnlyList<BoundedContextListItemResult> Items,
    int Total,
    int Page,
    int PageSize);

public sealed record BoundedContextListItemResult(
    Guid Id,
    string TechnicalName,
    string OwnerTeam,
    string Status,
    string? BusinessPurpose,
    string? DomainType,
    string? RoleType,
    int ResponsibilityCount,
    int RelationshipCount,
    DateTime UpdatedAt);
