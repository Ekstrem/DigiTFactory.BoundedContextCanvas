using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record MapRelationshipCommand(
    Guid AggregateId,
    Guid TargetContextId,
    string TargetContextName,
    string Direction,
    string Pattern) : IRequest<BoundedContextCanvasOperationResult>;
