using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record RemoveTermFromLanguageCommand(
    Guid AggregateId,
    string Term) : IRequest<BoundedContextCanvasOperationResult>;
