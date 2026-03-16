using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record ArchiveBoundedContextCommand(
    Guid AggregateId) : IRequest<BoundedContextCanvasOperationResult>;
