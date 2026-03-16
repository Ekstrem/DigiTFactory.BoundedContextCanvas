using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class RefineBoundaryHandler : IRequestHandler<RefineBoundaryCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public RefineBoundaryHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(RefineBoundaryCommand request, CancellationToken ct)
    {
        var aggregate = await _provider.GetAggregateAsync(request.AggregateId, ct);

        var responsibilities = request.Responsibilities
            .Select(r => Responsibility.CreateInstance(
                r.Description,
                Enum.Parse<ResponsibilityTypeEnum>(r.Type, ignoreCase: true)))
            .ToList<Domain.Abstraction.IResponsibility>()
            .AsReadOnly();

        var model = BoundedContextCanvasAnemicModel.Create(
            BoundedContextCanvasRoot.CreateInstance(request.AggregateId, string.Empty, string.Empty, CanvasStatusEnum.Draft),
            responsibilities: responsibilities);

        var result = aggregate.RefineBoundary(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(request.AggregateId, 2);
    }
}
