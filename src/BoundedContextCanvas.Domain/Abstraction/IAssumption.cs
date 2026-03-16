using Hive.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IAssumption : IValueObject
{
    string Statement { get; }
    RiskLevelEnum Risk { get; }
}
