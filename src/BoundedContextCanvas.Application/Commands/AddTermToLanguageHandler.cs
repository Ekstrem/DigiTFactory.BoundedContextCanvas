using MediatR;
using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed class AddTermToLanguageHandler : IRequestHandler<AddTermToLanguageCommand, BoundedContextCanvasOperationResult>
{
    private readonly AggregateProvider _provider;

    public AddTermToLanguageHandler(AggregateProvider provider) => _provider = provider;

    public async Task<BoundedContextCanvasOperationResult> Handle(AddTermToLanguageCommand request, CancellationToken ct)
    {
        var aggregate = await _provider.GetAggregateAsync(request.AggregateId, ct);

        var term = UbiquitousLanguageTerm.CreateInstance(request.Term, request.Definition);

        var language = new List<Domain.Abstraction.IUbiquitousLanguageTerm> { term }.AsReadOnly();

        var model = BoundedContextCanvasAnemicModel.Create(
            BoundedContextCanvasRoot.CreateInstance(request.AggregateId, string.Empty, string.Empty, CanvasStatusEnum.Draft),
            language: language);

        var result = aggregate.AddTermToLanguage(model);

        _provider.Notifier.Notify(result);

        return BoundedContextCanvasOperationResult.Success(request.AggregateId, 2);
    }
}
