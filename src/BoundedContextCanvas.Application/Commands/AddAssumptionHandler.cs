using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class AddAssumptionHandler : IRequestHandler<AddAssumptionCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public AddAssumptionHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(AddAssumptionCommand request, CancellationToken ct)
    {
        var aggregate = await _provider.GetAggregateAsync(request.AggregateId, ct);

        var risk = Enum.Parse<RiskLevelEnum>(request.Risk, ignoreCase: true);
        var assumption = Assumption.CreateInstance(request.Statement, risk);

        var assumptions = new List<Domain.Abstraction.IAssumption> { assumption }.AsReadOnly();

        var model = BoundedContextCanvasAnemicModel.Create(
            BoundedContextCanvasRoot.CreateInstance(request.AggregateId, string.Empty, string.Empty, CanvasStatusEnum.Draft),
            assumptions: assumptions);

        var result = aggregate.AddAssumption(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(request.AggregateId, 2);
    }
}
