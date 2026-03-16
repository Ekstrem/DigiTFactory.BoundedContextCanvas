using BoundedContextCanvas.Domain.Abstraction;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class Responsibility : IResponsibility
{
    public string Description { get; }
    public ResponsibilityTypeEnum Type { get; }

    private Responsibility(string description, ResponsibilityTypeEnum type)
    {
        Description = description;
        Type = type;
    }

    public static Responsibility CreateInstance(string description, ResponsibilityTypeEnum type)
        => new(description, type);
}
