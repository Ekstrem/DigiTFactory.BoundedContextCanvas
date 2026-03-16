using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class PublishContractHandler : IRequestHandler<PublishContractCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public PublishContractHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(PublishContractCommand request, CancellationToken ct)
    {
        var aggregate = await _provider.GetAggregateAsync(request.AggregateId, ct);

        var publicInterface = request.Items
            .Select(i => PublicInterfaceItem.CreateInstance(
                i.Name,
                Enum.Parse<InterfaceItemTypeEnum>(i.Type, ignoreCase: true),
                Enum.Parse<InterfaceDirectionEnum>(i.Direction, ignoreCase: true),
                i.LinkedResponsibility))
            .ToList<Domain.Abstraction.IPublicInterfaceItem>()
            .AsReadOnly();

        var model = BoundedContextCanvasAnemicModel.Create(
            BoundedContextCanvasRoot.CreateInstance(request.AggregateId, string.Empty, string.Empty, CanvasStatusEnum.Draft),
            publicInterface: publicInterface);

        var result = aggregate.PublishContract(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(request.AggregateId, 2);
    }
}
