using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class RemoveRelationshipHandler : IRequestHandler<RemoveRelationshipCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public RemoveRelationshipHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(RemoveRelationshipCommand request, CancellationToken ct)
    {
        var aggregate = await _provider.GetAggregateAsync(request.AggregateId, ct);

        var connection = ExternalConnection.CreateInstance(
            request.TargetContextId, string.Empty, ConnectionDirectionEnum.Upstream, IntegrationPatternEnum.Conformist);

        var relationships = new List<Domain.Abstraction.IExternalConnection> { connection }.AsReadOnly();

        var model = BoundedContextCanvasAnemicModel.Create(
            BoundedContextCanvasRoot.CreateInstance(request.AggregateId, string.Empty, string.Empty, CanvasStatusEnum.Draft),
            relationships: relationships);

        var result = aggregate.RemoveRelationship(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(request.AggregateId, 2);
    }
}
