using BoundedContextCanvas.Domain.Abstraction;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class ExternalConnection : IExternalConnection
{
    public Guid TargetContextId { get; }
    public string TargetContextName { get; }
    public ConnectionDirectionEnum Direction { get; }
    public IntegrationPatternEnum Pattern { get; }

    private ExternalConnection(Guid targetContextId, string targetContextName, ConnectionDirectionEnum direction, IntegrationPatternEnum pattern)
    {
        TargetContextId = targetContextId;
        TargetContextName = targetContextName;
        Direction = direction;
        Pattern = pattern;
    }

    public static ExternalConnection CreateInstance(Guid targetContextId, string targetContextName, ConnectionDirectionEnum direction, IntegrationPatternEnum pattern)
        => new(targetContextId, targetContextName, direction, pattern);
}
