using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class DefineStrategicImportanceHandler : IRequestHandler<DefineStrategicImportanceCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public DefineStrategicImportanceHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(DefineStrategicImportanceCommand request, CancellationToken ct)
    {
        var aggregate = await _provider.GetAggregateAsync(request.AggregateId, ct);

        var domainType = Enum.Parse<DomainTypeEnum>(request.DomainType, ignoreCase: true);
        var businessModelRole = Enum.Parse<BusinessModelRoleEnum>(request.BusinessModelRole, ignoreCase: true);
        var evolution = Enum.Parse<EvolutionEnum>(request.Evolution, ignoreCase: true);
        var roleType = Enum.Parse<DomainRoleTypeEnum>(request.RoleType, ignoreCase: true);

        var classification = StrategicClassification.CreateInstance(domainType, businessModelRole, evolution);
        var domainRole = DomainRole.CreateInstance(roleType, request.RoleDescription);

        var model = BoundedContextCanvasAnemicModel.Create(
            BoundedContextCanvasRoot.CreateInstance(request.AggregateId, string.Empty, string.Empty, CanvasStatusEnum.Draft),
            strategicClassification: classification,
            domainRole: domainRole);

        var result = aggregate.DefineStrategicImportance(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(request.AggregateId, 2);
    }
}
