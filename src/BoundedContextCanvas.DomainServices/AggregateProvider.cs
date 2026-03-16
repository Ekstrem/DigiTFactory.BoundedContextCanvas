using Hive.SeedWorks.TacticalPatterns;
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
        var anemicModel = await _repository.GetByIdAsync(id, ct);
        var aggregate = Aggregate.Create(anemicModel);
        return aggregate;
    }

    public Aggregate CreateNew(IBoundedContextCanvasAnemicModel model)
    {
        return Aggregate.Create(model);
    }

    public BoundedContextCanvasNotifier Notifier => _notifier;
}
