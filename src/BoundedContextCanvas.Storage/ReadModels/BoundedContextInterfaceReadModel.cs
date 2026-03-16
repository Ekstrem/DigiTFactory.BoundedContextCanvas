using Hive.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.Storage.ReadModels;

public class BoundedContextInterfaceReadModel : IReadModel<IBoundedContextCanvas>
{
    public Guid ContextId { get; set; }
    public int Ordinal { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public string LinkedResponsibility { get; set; } = string.Empty;
}
