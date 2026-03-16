using BoundedContextCanvas.Domain.Abstraction;

namespace BoundedContextCanvas.Domain;

public sealed class BoundedContextCanvasAnemicModel : IBoundedContextCanvasAnemicModel
{
    public IBoundedContextCanvasRoot Root { get; }
    public IStrategicClassification? StrategicClassification { get; }
    public IDefinition? Definition { get; }
    public IDomainRole? DomainRole { get; }
    public IReadOnlyList<IUbiquitousLanguageTerm> Language { get; }
    public IReadOnlyList<IResponsibility> Responsibilities { get; }
    public IReadOnlyList<IExternalConnection> Relationships { get; }
    public IReadOnlyList<IPublicInterfaceItem> PublicInterface { get; }
    public IReadOnlyList<IBusinessDecision> BusinessDecisions { get; }
    public IReadOnlyList<IAssumption> Assumptions { get; }

    private BoundedContextCanvasAnemicModel(
        IBoundedContextCanvasRoot root,
        IStrategicClassification? strategicClassification,
        IDefinition? definition,
        IDomainRole? domainRole,
        IReadOnlyList<IUbiquitousLanguageTerm> language,
        IReadOnlyList<IResponsibility> responsibilities,
        IReadOnlyList<IExternalConnection> relationships,
        IReadOnlyList<IPublicInterfaceItem> publicInterface,
        IReadOnlyList<IBusinessDecision> businessDecisions,
        IReadOnlyList<IAssumption> assumptions)
    {
        Root = root;
        StrategicClassification = strategicClassification;
        Definition = definition;
        DomainRole = domainRole;
        Language = language;
        Responsibilities = responsibilities;
        Relationships = relationships;
        PublicInterface = publicInterface;
        BusinessDecisions = businessDecisions;
        Assumptions = assumptions;
    }

    public static BoundedContextCanvasAnemicModel Create(
        IBoundedContextCanvasRoot root,
        IStrategicClassification? strategicClassification = null,
        IDefinition? definition = null,
        IDomainRole? domainRole = null,
        IReadOnlyList<IUbiquitousLanguageTerm>? language = null,
        IReadOnlyList<IResponsibility>? responsibilities = null,
        IReadOnlyList<IExternalConnection>? relationships = null,
        IReadOnlyList<IPublicInterfaceItem>? publicInterface = null,
        IReadOnlyList<IBusinessDecision>? businessDecisions = null,
        IReadOnlyList<IAssumption>? assumptions = null)
        => new(root, strategicClassification, definition, domainRole,
               language ?? Array.Empty<IUbiquitousLanguageTerm>(),
               responsibilities ?? Array.Empty<IResponsibility>(),
               relationships ?? Array.Empty<IExternalConnection>(),
               publicInterface ?? Array.Empty<IPublicInterfaceItem>(),
               businessDecisions ?? Array.Empty<IBusinessDecision>(),
               assumptions ?? Array.Empty<IAssumption>());
}
