using MediatR;

namespace BoundedContextCanvas.Application.Queries;

public sealed record GetContextStatsQuery : IRequest<ContextStatsResult>;

public sealed record ContextStatsResult(
    int TotalContexts,
    DomainTypeStatsResult ByDomainType,
    StatusStatsResult ByStatus,
    double AvgResponsibilitiesPerContext,
    double AvgRelationshipsPerContext,
    int TotalRelationships);

public sealed record DomainTypeStatsResult(int Core, int Supporting, int Generic);
public sealed record StatusStatsResult(int Draft, int Defined, int Published, int Archived);
