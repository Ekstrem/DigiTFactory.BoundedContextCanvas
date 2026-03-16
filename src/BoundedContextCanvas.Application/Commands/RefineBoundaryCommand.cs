using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record RefineBoundaryCommand(
    Guid AggregateId,
    List<RefineBoundaryCommand.ResponsibilityItem> Responsibilities) : IRequest<BoundedContextCanvasOperationResult>
{
    public sealed record ResponsibilityItem(string Description, string Type);
}
