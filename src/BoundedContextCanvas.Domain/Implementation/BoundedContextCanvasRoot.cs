using BoundedContextCanvas.Domain.Abstraction;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class BoundedContextCanvasRoot : IBoundedContextCanvasRoot
{
    public Guid ContextId { get; }
    public string TechnicalName { get; }
    public string OwnerTeam { get; }
    public CanvasStatusEnum Status { get; }

    private BoundedContextCanvasRoot(Guid contextId, string technicalName, string ownerTeam, CanvasStatusEnum status)
    {
        ContextId = contextId;
        TechnicalName = technicalName;
        OwnerTeam = ownerTeam;
        Status = status;
    }

    public static BoundedContextCanvasRoot CreateInstance(Guid contextId, string technicalName, string ownerTeam, CanvasStatusEnum status)
        => new(contextId, technicalName, ownerTeam, status);
}
