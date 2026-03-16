using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IResponsibility : IValueObject
{
    string Description { get; }
    ResponsibilityTypeEnum Type { get; }
}
