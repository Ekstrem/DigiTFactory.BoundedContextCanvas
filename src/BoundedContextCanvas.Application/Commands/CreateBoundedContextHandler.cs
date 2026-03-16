using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class CreateBoundedContextHandler : IRequestHandler<CreateBoundedContextCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public CreateBoundedContextHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(CreateBoundedContextCommand request, CancellationToken ct)
    {
        var id = Guid.NewGuid();
        var root = BoundedContextCanvasRoot.CreateInstance(id, request.TechnicalName, request.OwnerTeam, CanvasStatusEnum.Draft);
        var definition = Definition.CreateInstance(request.BusinessPurpose);
        var model = BoundedContextCanvasAnemicModel.Create(root, definition: definition);

        var aggregate = _provider.CreateNew(model);
        var result = aggregate.CreateBoundedContext(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(id, 1);
    }
}
