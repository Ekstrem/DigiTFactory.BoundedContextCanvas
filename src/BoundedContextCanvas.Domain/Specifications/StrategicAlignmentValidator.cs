using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Specifications;

public class StrategicAlignmentValidator : IBusinessOperationValidator<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
{
    private string _reason = string.Empty;

    public string Reason => _reason;
    public DomainOperationResultEnum DomainResult => DomainOperationResultEnum.Exception;

    public bool IsSatisfiedBy(BusinessOperationData<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> obj)
    {
        var model = obj.Model;
        if (model.StrategicClassification is null)
            return true;

        if (model.StrategicClassification.DomainType != DomainTypeEnum.Core)
            return true;

        var violations = model.Relationships
            .Where(r => r.Pattern == IntegrationPatternEnum.Conformist)
            .Select(r => r.TargetContextName)
            .ToList();

        if (violations.Count == 0)
            return true;

        _reason = $"Core Domain cannot use Conformist pattern. " +
                  $"Violating relationships: {string.Join(", ", violations)}. " +
                  "Use AntiCorruptionLayer (ACL) instead.";
        return false;
    }
}
