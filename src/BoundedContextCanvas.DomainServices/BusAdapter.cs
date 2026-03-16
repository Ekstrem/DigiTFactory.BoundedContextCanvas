using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.Result;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.DomainServices;

public class BusAdapter : IObserver<AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>>
{
    private readonly IEventBus _eventBus;

    public BusAdapter(IEventBus eventBus) => _eventBus = eventBus;

    public void OnCompleted() { }
    public void OnError(Exception error) { }

    public void OnNext(AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> value)
    {
        _eventBus.PublishAsync(value.Event).ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
