using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Queries;

public sealed class GetBoundedContextDetailHandler : IRequestHandler<GetBoundedContextDetailQuery, BoundedContextDetailResult?>
{
    private readonly IBoundedContextCanvasQueryRepository _repository;

    public GetBoundedContextDetailHandler(IBoundedContextCanvasQueryRepository repository)
        => _repository = repository;

    public async Task<BoundedContextDetailResult?> Handle(GetBoundedContextDetailQuery request, CancellationToken ct)
    {
        return await _repository.GetByIdAsync<BoundedContextDetailResult>(request.Id, ct);
    }
}
