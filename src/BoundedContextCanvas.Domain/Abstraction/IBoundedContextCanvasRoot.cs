using Hive.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IBoundedContextCanvasRoot : IAggregateRoot<IBoundedContextCanvas>
{
    Guid ContextId { get; }
    string TechnicalName { get; }
    string OwnerTeam { get; }
    CanvasStatusEnum Status { get; }
}
