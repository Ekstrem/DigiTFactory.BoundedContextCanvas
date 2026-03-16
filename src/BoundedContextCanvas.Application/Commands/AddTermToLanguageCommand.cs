using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record AddTermToLanguageCommand(
    Guid AggregateId,
    string Term,
    string Definition) : IRequest<BoundedContextCanvasOperationResult>;
