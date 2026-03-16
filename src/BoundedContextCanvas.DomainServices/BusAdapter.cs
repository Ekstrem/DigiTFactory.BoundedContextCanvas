using DigiTFactory.Libraries.SeedWorks.Events;
using DigiTFactory.Libraries.SeedWorks.Result;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.DomainServices;

public class BusAdapter : IObserver<AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>>
{
    private readonly IEventBus _eventBus;

    public BusAdapter(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> value)
    {
        // Publish domain event to the bus
        // The event bus handles persistence to Command DB, projection to Read Store, and outbound handlers
        _eventBus.PublishAsync(value).ConfigureAwait(false).GetAwaiter().GetResult();
    }
}
