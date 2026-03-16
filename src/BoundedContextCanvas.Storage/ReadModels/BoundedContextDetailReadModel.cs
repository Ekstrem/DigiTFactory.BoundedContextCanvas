using Hive.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.Storage.ReadModels;

public class BoundedContextDetailReadModel : IReadModel<IBoundedContextCanvas>
{
    public Guid Id { get; set; }
    public string TechnicalName { get; set; } = string.Empty;
    public string OwnerTeam { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? BusinessPurpose { get; set; }
    public string? DomainType { get; set; }
    public string? BusinessModelRole { get; set; }
    public string? Evolution { get; set; }
    public string? RoleType { get; set; }
    public string? RoleDescription { get; set; }
    public string? BusinessDecisionsJson { get; set; }
    public string? AssumptionsJson { get; set; }
    public long Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
