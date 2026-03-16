using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IPublicInterfaceItem : IValueObject
{
    string Name { get; }
    InterfaceItemTypeEnum Type { get; }
    InterfaceDirectionEnum Direction { get; }
    string LinkedResponsibility { get; }
}
