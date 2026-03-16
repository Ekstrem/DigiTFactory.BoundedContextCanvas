using Hive.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IExternalConnection : IValueObject
{
    Guid TargetContextId { get; }
    string TargetContextName { get; }
    ConnectionDirectionEnum Direction { get; }
    IntegrationPatternEnum Pattern { get; }
}
