using Hive.SeedWorks.TacticalPatterns;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IBusinessDecision : IValueObject
{
    string Rule { get; }
    string Rationale { get; }
}
