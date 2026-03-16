using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IStrategicClassification : IValueObject
{
    DomainTypeEnum DomainType { get; }
    BusinessModelRoleEnum BusinessModelRole { get; }
    EvolutionEnum Evolution { get; }
}
