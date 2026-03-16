using Hive.SeedWorks.Result;
using Hive.SeedWorks.TacticalPatterns;
using BoundedContextCanvas.Domain.Abstraction;
using BoundedContextCanvas.Domain.Enums;
using BoundedContextCanvas.Domain.Implementation;

namespace BoundedContextCanvas.Domain;

public class Aggregate
{
    private IBoundedContextCanvasAnemicModel _state;

    private Aggregate(IBoundedContextCanvasAnemicModel state)
    {
        _state = state;
    }

    public static Aggregate Create(IBoundedContextCanvasAnemicModel state) => new(state);

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> CreateBoundedContext(
        IBoundedContextCanvasAnemicModel model)
    {
        // model already contains Root with Draft status and Definition
        _state = model;
        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> DefineStrategicImportance(
        IBoundedContextCanvasAnemicModel model)
    {
        var newRoot = BoundedContextCanvasRoot.CreateInstance(
            _state.Root.ContextId,
            _state.Root.TechnicalName,
            _state.Root.OwnerTeam,
            _state.Root.Status == CanvasStatusEnum.Draft ? CanvasStatusEnum.Defined : _state.Root.Status);

        _state = BoundedContextCanvasAnemicModel.Create(
            newRoot,
            model.StrategicClassification,
            _state.Definition,
            model.DomainRole,
            _state.Language,
            _state.Responsibilities,
            _state.Relationships,
            _state.PublicInterface,
            _state.BusinessDecisions,
            _state.Assumptions);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> RefineBoundary(
        IBoundedContextCanvasAnemicModel model)
    {
        _state = BoundedContextCanvasAnemicModel.Create(
            _state.Root, _state.StrategicClassification, _state.Definition, _state.DomainRole,
            _state.Language, model.Responsibilities, _state.Relationships,
            _state.PublicInterface, _state.BusinessDecisions, _state.Assumptions);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> AddTermToLanguage(
        IBoundedContextCanvasAnemicModel model)
    {
        var newTerm = model.Language.Last();
        var updatedLanguage = _state.Language.Append(newTerm).ToList().AsReadOnly();

        _state = BoundedContextCanvasAnemicModel.Create(
            _state.Root, _state.StrategicClassification, _state.Definition, _state.DomainRole,
            updatedLanguage, _state.Responsibilities, _state.Relationships,
            _state.PublicInterface, _state.BusinessDecisions, _state.Assumptions);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> RemoveTermFromLanguage(
        IBoundedContextCanvasAnemicModel model)
    {
        var termToRemove = model.Language.Last().Term;
        var updatedLanguage = _state.Language
            .Where(t => !t.Term.Equals(termToRemove, StringComparison.OrdinalIgnoreCase))
            .ToList().AsReadOnly();

        _state = BoundedContextCanvasAnemicModel.Create(
            _state.Root, _state.StrategicClassification, _state.Definition, _state.DomainRole,
            updatedLanguage, _state.Responsibilities, _state.Relationships,
            _state.PublicInterface, _state.BusinessDecisions, _state.Assumptions);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> MapRelationship(
        IBoundedContextCanvasAnemicModel model)
    {
        var newConnection = model.Relationships.Last();
        var updated = _state.Relationships
            .Where(r => r.TargetContextId != newConnection.TargetContextId)
            .Append(newConnection).ToList().AsReadOnly();

        _state = BoundedContextCanvasAnemicModel.Create(
            _state.Root, _state.StrategicClassification, _state.Definition, _state.DomainRole,
            _state.Language, _state.Responsibilities, updated,
            _state.PublicInterface, _state.BusinessDecisions, _state.Assumptions);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> RemoveRelationship(
        IBoundedContextCanvasAnemicModel model)
    {
        var targetId = model.Relationships.Last().TargetContextId;
        var updated = _state.Relationships
            .Where(r => r.TargetContextId != targetId)
            .ToList().AsReadOnly();

        _state = BoundedContextCanvasAnemicModel.Create(
            _state.Root, _state.StrategicClassification, _state.Definition, _state.DomainRole,
            _state.Language, _state.Responsibilities, updated,
            _state.PublicInterface, _state.BusinessDecisions, _state.Assumptions);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> PublishContract(
        IBoundedContextCanvasAnemicModel model)
    {
        var newRoot = BoundedContextCanvasRoot.CreateInstance(
            _state.Root.ContextId, _state.Root.TechnicalName, _state.Root.OwnerTeam,
            CanvasStatusEnum.Published);

        _state = BoundedContextCanvasAnemicModel.Create(
            newRoot, _state.StrategicClassification, _state.Definition, _state.DomainRole,
            _state.Language, _state.Responsibilities, _state.Relationships,
            model.PublicInterface, _state.BusinessDecisions, _state.Assumptions);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> AddBusinessDecision(
        IBoundedContextCanvasAnemicModel model)
    {
        var newDecision = model.BusinessDecisions.Last();
        var updated = _state.BusinessDecisions.Append(newDecision).ToList().AsReadOnly();

        _state = BoundedContextCanvasAnemicModel.Create(
            _state.Root, _state.StrategicClassification, _state.Definition, _state.DomainRole,
            _state.Language, _state.Responsibilities, _state.Relationships,
            _state.PublicInterface, updated, _state.Assumptions);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> AddAssumption(
        IBoundedContextCanvasAnemicModel model)
    {
        var newAssumption = model.Assumptions.Last();
        var updated = _state.Assumptions.Append(newAssumption).ToList().AsReadOnly();

        _state = BoundedContextCanvasAnemicModel.Create(
            _state.Root, _state.StrategicClassification, _state.Definition, _state.DomainRole,
            _state.Language, _state.Responsibilities, _state.Relationships,
            _state.PublicInterface, _state.BusinessDecisions, updated);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }

    public AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> ArchiveBoundedContext(
        IBoundedContextCanvasAnemicModel model)
    {
        var newRoot = BoundedContextCanvasRoot.CreateInstance(
            _state.Root.ContextId, _state.Root.TechnicalName, _state.Root.OwnerTeam,
            CanvasStatusEnum.Archived);

        _state = BoundedContextCanvasAnemicModel.Create(
            newRoot, _state.StrategicClassification, _state.Definition, _state.DomainRole,
            _state.Language, _state.Responsibilities, _state.Relationships,
            _state.PublicInterface, _state.BusinessDecisions, _state.Assumptions);

        return AggregateResult<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
            .Success(_state);
    }
}
