using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class RemoveTermFromLanguageHandler : IRequestHandler<RemoveTermFromLanguageCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public RemoveTermFromLanguageHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(RemoveTermFromLanguageCommand request, CancellationToken ct)
    {
        var aggregate = await _provider.GetAggregateAsync(request.AggregateId, ct);

        var term = UbiquitousLanguageTerm.CreateInstance(request.Term, string.Empty);

        var language = new List<Domain.Abstraction.IUbiquitousLanguageTerm> { term }.AsReadOnly();

        var model = BoundedContextCanvasAnemicModel.Create(
            BoundedContextCanvasRoot.CreateInstance(request.AggregateId, string.Empty, string.Empty, CanvasStatusEnum.Draft),
            language: language);

        var result = aggregate.RemoveTermFromLanguage(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(request.AggregateId, 2);
    }
}
