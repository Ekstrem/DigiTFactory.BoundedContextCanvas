using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.Storage.ReadModels;

public class BoundedContextResponsibilityReadModel : IReadModel<IBoundedContextCanvas>
{
    public Guid ContextId { get; set; }
    public int Ordinal { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
