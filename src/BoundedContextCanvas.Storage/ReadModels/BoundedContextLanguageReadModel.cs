using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.Storage.ReadModels;

public class BoundedContextLanguageReadModel : IReadModel<IBoundedContextCanvas>
{
    public Guid ContextId { get; set; }
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
}
