using DigiTFactory.Libraries.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain.Abstraction;

namespace BoundedContextCanvas.Domain;

public interface IBoundedContextCanvasAnemicModel : IAnemicModel<IBoundedContextCanvas>
{
    IBoundedContextCanvasRoot Root { get; }
    IStrategicClassification? StrategicClassification { get; }
    IDefinition? Definition { get; }
    IDomainRole? DomainRole { get; }
    IReadOnlyList<IUbiquitousLanguageTerm> Language { get; }
    IReadOnlyList<IResponsibility> Responsibilities { get; }
    IReadOnlyList<IExternalConnection> Relationships { get; }
    IReadOnlyList<IPublicInterfaceItem> PublicInterface { get; }
    IReadOnlyList<IBusinessDecision> BusinessDecisions { get; }
    IReadOnlyList<IAssumption> Assumptions { get; }
}
