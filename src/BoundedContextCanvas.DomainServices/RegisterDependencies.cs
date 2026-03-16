using Autofac;
using BoundedContextCanvas.Domain;

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
            .SingleInstance();
    }
}
