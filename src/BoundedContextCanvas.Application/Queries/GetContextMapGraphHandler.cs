using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Queries;

public sealed class GetContextMapGraphHandler : IRequestHandler<GetContextMapGraphQuery, ContextMapGraphResult>
{
    private readonly IBoundedContextCanvasQueryRepository _repository;

    public GetContextMapGraphHandler(IBoundedContextCanvasQueryRepository repository)
        => _repository = repository;

    public async Task<ContextMapGraphResult> Handle(GetContextMapGraphQuery request, CancellationToken ct)
    {
        var nodes = await _repository.QueryAsync<ContextNodeResult>(ct: ct);
        var edges = await _repository.QueryAsync<ContextEdgeResult>(ct: ct);

        return new ContextMapGraphResult(nodes, edges);
    }
}
