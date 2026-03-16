using Hive.SeedWorks.TacticalPatterns.Specifications;
using Hive.SeedWorks.Result;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Specifications;

public class StrategicAlignmentValidator : IBusinessOperationValidator<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
{
    public DomainResult IsSatisfiedBy(IBoundedContextCanvasAnemicModel model)
    {
        if (model.StrategicClassification is null)
            return DomainResult.Success();

        if (model.StrategicClassification.DomainType != DomainTypeEnum.Core)
            return DomainResult.Success();

        var violations = model.Relationships
            .Where(r => r.Pattern == IntegrationPatternEnum.Conformist)
            .Select(r => r.TargetContextName)
            .ToList();

        if (violations.Count == 0)
            return DomainResult.Success();

        return DomainResult.Exception(
            $"Core Domain cannot use Conformist pattern. " +
            $"Violating relationships: {string.Join(", ", violations)}. " +
            "Use AntiCorruptionLayer (ACL) instead.");
    }
}
