using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class AddBusinessDecisionHandler : IRequestHandler<AddBusinessDecisionCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public AddBusinessDecisionHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(AddBusinessDecisionCommand request, CancellationToken ct)
    {
        var aggregate = await _provider.GetAggregateAsync(request.AggregateId, ct);

        var decision = BusinessDecision.CreateInstance(request.Rule, request.Rationale);

        var decisions = new List<Domain.Abstraction.IBusinessDecision> { decision }.AsReadOnly();

        var model = BoundedContextCanvasAnemicModel.Create(
            BoundedContextCanvasRoot.CreateInstance(request.AggregateId, string.Empty, string.Empty, CanvasStatusEnum.Draft),
            businessDecisions: decisions);

        var result = aggregate.AddBusinessDecision(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(request.AggregateId, 2);
    }
}
