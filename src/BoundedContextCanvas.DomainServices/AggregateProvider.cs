using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.DomainServices;

public class AggregateProvider
{
    private readonly IAnemicModelRepository<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> _repository;
    private readonly BoundedContextCanvasNotifier _notifier;

    public AggregateProvider(
        IAnemicModelRepository<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> repository,
        BoundedContextCanvasNotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    public async Task<Aggregate> GetAggregateAsync(Guid id, CancellationToken ct = default)
    {
        var versions = await _repository.GetById(id, ct);
        var latest = versions.OrderByDescending(v => v.Version).First();
        return Aggregate.Create(latest);
    }

    public Aggregate CreateNew(IBoundedContextCanvasAnemicModel model)
        => Aggregate.Create(model);

    public BoundedContextCanvasNotifier Notifier => _notifier;
}
