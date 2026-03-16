using Autofac;
using BoundedContextCanvas.Domain;
using Hive.SeedWorks.Events;

namespace BoundedContextCanvas.DomainServices;

public class RegisterDependencies : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<BoundedContextCanvasNotifier>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<BusAdapter>()
            .AsSelf()
            .SingleInstance();

        builder.RegisterType<AggregateProvider>()
            .AsSelf()
            .InstancePerLifetimeScope();

        builder.Register(ctx =>
        {
            var notifier = ctx.Resolve<BoundedContextCanvasNotifier>();
            var busAdapter = ctx.Resolve<BusAdapter>();
            notifier.Subscribe(busAdapter);
            return notifier;
        })
        .AsSelf()
        .SingleInstance()
        .AutoActivate();
    }
}
