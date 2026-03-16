using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record RemoveRelationshipCommand(
    Guid AggregateId,
    Guid TargetContextId) : IRequest<BoundedContextCanvasOperationResult>;
