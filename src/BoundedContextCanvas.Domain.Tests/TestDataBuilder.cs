using BoundedContextCanvas.Domain;
using BoundedContextCanvas.Domain.Abstraction;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;

namespace BoundedContextCanvas.Domain.Tests;

public static class TestDataBuilder
{
    public static IBoundedContextCanvasRoot CreateRoot(
        Guid? contextId = null,
        string technicalName = "TestContext",
        string ownerTeam = "PlatformTeam",
        CanvasStatusEnum status = CanvasStatusEnum.Draft)
        => BoundedContextCanvasRoot.CreateInstance(
            contextId ?? Guid.NewGuid(), technicalName, ownerTeam, status);

    public static IDefinition CreateDefinition(string purpose = "Test bounded context for unit testing")
        => Definition.CreateInstance(purpose);

    public static IStrategicClassification CreateStrategicClassification(
        DomainTypeEnum domainType = DomainTypeEnum.Supporting,
        BusinessModelRoleEnum role = BusinessModelRoleEnum.EngagementCreator,
        EvolutionEnum evolution = EvolutionEnum.CustomBuilt)
        => StrategicClassification.CreateInstance(domainType, role, evolution);

    public static IDomainRole CreateDomainRole(
        DomainRoleTypeEnum roleType = DomainRoleTypeEnum.Execution,
        string description = "Executes business operations")
        => DomainRole.CreateInstance(roleType, description);

    public static IResponsibility CreateResponsibility(
        string description = "Handle business logic",
        ResponsibilityTypeEnum type = ResponsibilityTypeEnum.Does)
        => Responsibility.CreateInstance(description, type);

    public static IUbiquitousLanguageTerm CreateTerm(
        string term = "Aggregate",
        string definition = "A cluster of domain objects")
        => UbiquitousLanguageTerm.CreateInstance(term, definition);

    public static IExternalConnection CreateConnection(
        Guid? targetId = null,
        string targetName = "OtherContext",
        ConnectionDirectionEnum direction = ConnectionDirectionEnum.Downstream,
        IntegrationPatternEnum pattern = IntegrationPatternEnum.ACL)
        => ExternalConnection.CreateInstance(
            targetId ?? Guid.NewGuid(), targetName, direction, pattern);

    public static IPublicInterfaceItem CreateInterfaceItem(
        string name = "TestCommand",
        InterfaceItemTypeEnum type = InterfaceItemTypeEnum.Command,
        InterfaceDirectionEnum direction = InterfaceDirectionEnum.Inbound,
        string linkedResponsibility = "Handle business logic")
        => PublicInterfaceItem.CreateInstance(name, type, direction, linkedResponsibility);

    public static IBusinessDecision CreateBusinessDecision(
        string rule = "Test rule",
        string rationale = "Test rationale")
        => BusinessDecision.CreateInstance(rule, rationale);

    public static IAssumption CreateAssumption(
        string statement = "Test assumption",
        RiskLevelEnum risk = RiskLevelEnum.Low)
        => Assumption.CreateInstance(statement, risk);

    public static BoundedContextCanvasAnemicModel CreateDefaultModel(
        IBoundedContextCanvasRoot? root = null,
        IDefinition? definition = null)
        => BoundedContextCanvasAnemicModel.Create(
            root ?? CreateRoot(),
            definition: definition ?? CreateDefinition());

    public static BoundedContextCanvasAnemicModel CreateDefinedModel(Guid? id = null)
    {
        var contextId = id ?? Guid.NewGuid();
        return BoundedContextCanvasAnemicModel.Create(
            CreateRoot(contextId, status: CanvasStatusEnum.Defined),
            CreateStrategicClassification(),
            CreateDefinition(),
            CreateDomainRole(),
            new[] { CreateTerm() },
            new[] { CreateResponsibility() },
            Array.Empty<IExternalConnection>(),
            Array.Empty<IPublicInterfaceItem>(),
            Array.Empty<IBusinessDecision>(),
            Array.Empty<IAssumption>());
    }
}
