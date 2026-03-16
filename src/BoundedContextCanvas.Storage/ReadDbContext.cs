using Microsoft.EntityFrameworkCore;
using BoundedContextCanvas.Storage.ReadModels;

namespace BoundedContextCanvas.Storage;

public class ReadDbContext : DbContext
{
    public ReadDbContext(DbContextOptions<ReadDbContext> options) : base(options) { }

    public DbSet<BoundedContextDetailReadModel> BoundedContextDetails => Set<BoundedContextDetailReadModel>();
    public DbSet<BoundedContextListReadModel> BoundedContextList => Set<BoundedContextListReadModel>();
    public DbSet<BoundedContextLanguageReadModel> BoundedContextLanguage => Set<BoundedContextLanguageReadModel>();
    public DbSet<BoundedContextResponsibilityReadModel> BoundedContextResponsibilities => Set<BoundedContextResponsibilityReadModel>();
    public DbSet<BoundedContextRelationshipReadModel> BoundedContextRelationships => Set<BoundedContextRelationshipReadModel>();
    public DbSet<BoundedContextInterfaceReadModel> BoundedContextInterface => Set<BoundedContextInterfaceReadModel>();
    public DbSet<ContextRelationshipReadModel> ContextRelationships => Set<ContextRelationshipReadModel>();
    public DbSet<ContextStatsReadModel> ContextStats => Set<ContextStatsReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("read_model");

        modelBuilder.Entity<BoundedContextDetailReadModel>(e =>
        {
            e.ToTable("bounded_context_details");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.TechnicalName).IsUnique();
            e.HasIndex(x => x.OwnerTeam);
        });

        modelBuilder.Entity<BoundedContextListReadModel>(e =>
        {
            e.ToTable("bounded_context_list");
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.OwnerTeam);
            e.HasIndex(x => x.DomainType);
            e.HasIndex(x => x.Status);
            e.HasIndex(x => x.UpdatedAt).IsDescending();
        });

        modelBuilder.Entity<BoundedContextLanguageReadModel>(e =>
        {
            e.ToTable("bounded_context_language");
            e.HasKey(x => new { x.ContextId, x.Term });
            e.HasIndex(x => x.ContextId);
        });

        modelBuilder.Entity<BoundedContextResponsibilityReadModel>(e =>
        {
            e.ToTable("bounded_context_responsibilities");
            e.HasKey(x => new { x.ContextId, x.Ordinal });
            e.HasIndex(x => x.ContextId);
        });

        modelBuilder.Entity<BoundedContextRelationshipReadModel>(e =>
        {
            e.ToTable("bounded_context_relationships");
            e.HasKey(x => new { x.ContextId, x.TargetContextId });
            e.HasIndex(x => x.ContextId);
            e.HasIndex(x => x.TargetContextId);
        });

        modelBuilder.Entity<BoundedContextInterfaceReadModel>(e =>
        {
            e.ToTable("bounded_context_interface");
            e.HasKey(x => new { x.ContextId, x.Ordinal });
            e.HasIndex(x => x.ContextId);
        });

        modelBuilder.Entity<ContextRelationshipReadModel>(e =>
        {
            e.ToTable("context_relationships");
            e.HasKey(x => new { x.SourceContextId, x.TargetContextId });
            e.HasIndex(x => x.TargetContextId);
        });

        modelBuilder.Entity<ContextStatsReadModel>(e =>
        {
            e.ToTable("context_stats");
            e.HasKey(x => x.Id);
        });
    }
}
