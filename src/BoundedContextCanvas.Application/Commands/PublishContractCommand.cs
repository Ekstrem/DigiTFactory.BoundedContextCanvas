using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record PublishContractCommand(
    Guid AggregateId,
    List<PublishContractCommand.InterfaceItem> Items) : IRequest<BoundedContextCanvasOperationResult>
{
    public sealed record InterfaceItem(string Name, string Type, string Direction, string LinkedResponsibility);
}
