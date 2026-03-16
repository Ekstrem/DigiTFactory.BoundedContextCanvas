using MediatR;

namespace BoundedContextCanvas.Application.Queries;

public sealed record GetContextMapGraphQuery : IRequest<ContextMapGraphResult>;

public sealed record ContextMapGraphResult(
    IReadOnlyList<ContextNodeResult> Nodes,
    IReadOnlyList<ContextEdgeResult> Edges);

public sealed record ContextNodeResult(Guid ContextId, string TechnicalName, string? DomainType);
public sealed record ContextEdgeResult(Guid SourceContextId, Guid TargetContextId, string Direction, string Pattern);
