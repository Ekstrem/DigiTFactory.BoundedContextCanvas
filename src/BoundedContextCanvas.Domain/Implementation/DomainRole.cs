using BoundedContextCanvas.Domain.Abstraction;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class DomainRole : IDomainRole
{
    public DomainRoleTypeEnum RoleType { get; }
    public string Description { get; }

    private DomainRole(DomainRoleTypeEnum roleType, string description)
    {
        RoleType = roleType;
        Description = description;
    }

    public static DomainRole CreateInstance(DomainRoleTypeEnum roleType, string description)
        => new(roleType, description);
}
