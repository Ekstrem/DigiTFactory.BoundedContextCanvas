using BoundedContextCanvas.Domain.Abstraction;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Implementation;

public sealed class StrategicClassification : IStrategicClassification
{
    public DomainTypeEnum DomainType { get; }
    public BusinessModelRoleEnum BusinessModelRole { get; }
    public EvolutionEnum Evolution { get; }

    private StrategicClassification(DomainTypeEnum domainType, BusinessModelRoleEnum businessModelRole, EvolutionEnum evolution)
    {
        DomainType = domainType;
        BusinessModelRole = businessModelRole;
        Evolution = evolution;
    }

    public static StrategicClassification CreateInstance(DomainTypeEnum domainType, BusinessModelRoleEnum businessModelRole, EvolutionEnum evolution)
        => new(domainType, businessModelRole, evolution);
}
