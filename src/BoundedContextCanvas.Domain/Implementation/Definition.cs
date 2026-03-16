using BoundedContextCanvas.Domain.Abstraction;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class Definition : IDefinition
{
    public string BusinessPurpose { get; }

    private Definition(string businessPurpose)
    {
        BusinessPurpose = businessPurpose;
    }

    public static Definition CreateInstance(string businessPurpose)
        => new(businessPurpose);
}
