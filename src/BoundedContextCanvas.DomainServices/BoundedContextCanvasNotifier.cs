using DigiTFactory.Libraries.SeedWorks.Result;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.DomainServices;

public class BoundedContextCanvasNotifier : IObservable<AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>>
{
    private readonly List<IObserver<AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>>> _observers = new();

    public IDisposable Subscribe(IObserver<AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>> observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void Notify(AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> result)
    {
        foreach (var observer in _observers)
            observer.OnNext(result);
    }

    private sealed class Unsubscriber : IDisposable
    {
        private readonly List<IObserver<AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>>> _observers;
        private readonly IObserver<AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>> _observer;

        public Unsubscriber(
            List<IObserver<AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>>> observers,
            IObserver<AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            _observers.Remove(_observer);
        }
    }
}
