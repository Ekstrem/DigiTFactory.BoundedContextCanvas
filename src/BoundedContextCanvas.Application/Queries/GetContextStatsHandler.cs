using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Queries;

public sealed class GetContextStatsHandler : IRequestHandler<GetContextStatsQuery, ContextStatsResult>
{
    private readonly IBoundedContextCanvasQueryRepository _repository;

    public GetContextStatsHandler(IBoundedContextCanvasQueryRepository repository)
        => _repository = repository;

    public async Task<ContextStatsResult> Handle(GetContextStatsQuery request, CancellationToken ct)
    {
        return await _repository.GetSingleAsync<ContextStatsResult>(ct)
            ?? new ContextStatsResult(
                TotalContexts: 0,
                ByDomainType: new DomainTypeStatsResult(0, 0, 0),
                ByStatus: new StatusStatsResult(0, 0, 0, 0),
                AvgResponsibilitiesPerContext: 0,
                AvgRelationshipsPerContext: 0,
                TotalRelationships: 0);
    }
}
