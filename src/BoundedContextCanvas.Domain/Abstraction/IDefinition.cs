using Hive.SeedWorks.TacticalPatterns;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IDefinition : IValueObject
{
    string BusinessPurpose { get; }
}
