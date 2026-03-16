using Hive.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.Storage.ReadModels;

public class ContextStatsReadModel : IReadModel<IBoundedContextCanvas>
{
    public string Id { get; set; } = "singleton";
    public int TotalContexts { get; set; }
    public int CoreCount { get; set; }
    public int SupportingCount { get; set; }
    public int GenericCount { get; set; }
    public int DraftCount { get; set; }
    public int DefinedCount { get; set; }
    public int PublishedCount { get; set; }
    public int ArchivedCount { get; set; }
    public int TotalResponsibilities { get; set; }
    public int TotalRelationships { get; set; }
}
