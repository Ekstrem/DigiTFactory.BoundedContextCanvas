using Hive.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.Storage.ReadModels;

public class BoundedContextListReadModel : IReadModel<IBoundedContextCanvas>
{
    public Guid Id { get; set; }
    public string TechnicalName { get; set; } = string.Empty;
    public string OwnerTeam { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? BusinessPurpose { get; set; }
    public string? DomainType { get; set; }
    public string? RoleType { get; set; }
    public int ResponsibilityCount { get; set; }
    public int RelationshipCount { get; set; }
    public DateTime UpdatedAt { get; set; }
}
