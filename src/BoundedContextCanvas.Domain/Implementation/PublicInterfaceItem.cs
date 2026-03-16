using BoundedContextCanvas.Domain.Abstraction;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class PublicInterfaceItem : IPublicInterfaceItem
{
    public string Name { get; }
    public InterfaceItemTypeEnum Type { get; }
    public InterfaceDirectionEnum Direction { get; }
    public string LinkedResponsibility { get; }

    private PublicInterfaceItem(string name, InterfaceItemTypeEnum type, InterfaceDirectionEnum direction, string linkedResponsibility)
    {
        Name = name;
        Type = type;
        Direction = direction;
        LinkedResponsibility = linkedResponsibility;
    }

    public static PublicInterfaceItem CreateInstance(string name, InterfaceItemTypeEnum type, InterfaceDirectionEnum direction, string linkedResponsibility)
        => new(name, type, direction, linkedResponsibility);
}
