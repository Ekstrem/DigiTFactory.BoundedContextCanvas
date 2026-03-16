using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IDefinition : IValueObject
{
    string BusinessPurpose { get; }
}
