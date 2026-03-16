using Autofac;
using Autofac.Extensions.DependencyInjection;
using BoundedContextCanvas.Application.Commands;
using BoundedContextCanvas.DomainServices;
using BoundedContextCanvas.InternalContracts;
using BoundedContextCanvas.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DigiTFactory.Libraries.EventBus.InMemory.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(cb =>
{
    cb.RegisterModule(new RegisterDependencies());
});

// EventBus (InMemory for Single profile)
builder.Services.AddEventBusInMemory();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateBoundedContextCommand).Assembly));

// EF Core — Read Store (PostgreSQL, Single profile uses same DB)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Host=localhost;Database=bounded_context_canvas;Username=postgres;Password=postgres";

builder.Services.AddDbContext<ReadDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IBoundedContextCanvasQueryRepository, QueryRepository>();

// Controllers
builder.Services.AddControllers();

// OpenAPI / Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BoundedContextCanvas API",
        Version = "v1",
        Description = "Управление жизненным циклом описаний ограниченных контекстов (Bounded Context Canvas). " +
                      "Стратегические артефакты для проектирования микросервисов.",
        Contact = new OpenApiContact
        {
            Name = "Platform Architecture Team"
        }
    });

    // XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Wire Observer: BusAdapter subscribes to Notifier
var notifier = app.Services.GetRequiredService<BoundedContextCanvasNotifier>();
var busAdapter = app.Services.GetRequiredService<BusAdapter>();
notifier.Subscribe(busAdapter);

// Swagger UI
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "BoundedContextCanvas API v1");
    options.RoutePrefix = "swagger";
});

app.MapControllers();

// Auto-migrate Read DB in development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ReadDbContext>();
    await db.Database.EnsureCreatedAsync();
}

await app.RunAsync();
