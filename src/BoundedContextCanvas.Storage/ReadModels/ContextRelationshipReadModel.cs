using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain;

namespace BoundedContextCanvas.Storage.ReadModels;

public class ContextRelationshipReadModel : IReadModel<IBoundedContextCanvas>
{
    public Guid SourceContextId { get; set; }
    public string SourceContextName { get; set; } = string.Empty;
    public string SourceDomainType { get; set; } = string.Empty;
    public Guid TargetContextId { get; set; }
    public string TargetContextName { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;
    public string Pattern { get; set; } = string.Empty;
}
