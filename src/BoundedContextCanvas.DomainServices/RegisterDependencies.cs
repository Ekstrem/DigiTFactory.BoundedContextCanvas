using Autofac;
using BoundedContextCanvas.Domain;
using DigiTFactory.Libraries.SeedWorks.Events;

namespace BoundedContextCanvas.DomainServices;

public class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BusAdapter>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.RegisterType<BoundedContextCanvasNotifier>()
            .AsSelf()
            .SingleInstance()
            .OnActivated(e =>
            {
                var busAdapter = e.Context.Resolve<BusAdapter>();
                e.Instance.Subscribe(busAdapter);
            });
    }
}
