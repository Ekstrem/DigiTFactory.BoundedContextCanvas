using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record AddBusinessDecisionCommand(
    Guid AggregateId,
    string Rule,
    string Rationale) : IRequest<BoundedContextCanvasOperationResult>;
