using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class MapRelationshipHandler : IRequestHandler<MapRelationshipCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public MapRelationshipHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(MapRelationshipCommand request, CancellationToken ct)
    {
        var aggregate = await _provider.GetAggregateAsync(request.AggregateId, ct);

        var direction = Enum.Parse<ConnectionDirectionEnum>(request.Direction, ignoreCase: true);
        var pattern = Enum.Parse<IntegrationPatternEnum>(request.Pattern, ignoreCase: true);

        var connection = ExternalConnection.CreateInstance(
            request.TargetContextId, request.TargetContextName, direction, pattern);

        var relationships = new List<Domain.Abstraction.IExternalConnection> { connection }.AsReadOnly();

        var model = BoundedContextCanvasAnemicModel.Create(
            BoundedContextCanvasRoot.CreateInstance(request.AggregateId, string.Empty, string.Empty, CanvasStatusEnum.Draft),
            relationships: relationships);

        var result = aggregate.MapRelationship(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(request.AggregateId, 2);
    }
}
