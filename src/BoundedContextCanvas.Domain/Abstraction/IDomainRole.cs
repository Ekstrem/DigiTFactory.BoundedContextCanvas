using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Abstraction;

public interface IDomainRole : IValueObject
{
    DomainRoleTypeEnum RoleType { get; }
    string Description { get; }
}
