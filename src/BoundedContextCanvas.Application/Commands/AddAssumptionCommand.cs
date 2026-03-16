using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record AddAssumptionCommand(
    Guid AggregateId,
    string Statement,
    string Risk) : IRequest<BoundedContextCanvasOperationResult>;
