using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;

namespace BoundedContextCanvas.Domain.Specifications;

public class ResponsibilityCohesionValidator : IBusinessOperationValidator<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
{
    public DomainResult IsSatisfiedBy(IBoundedContextCanvasAnemicModel model)
    {
        var duplicates = model.Responsibilities
            .GroupBy(r => r.Description, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicates.Count == 0)
            return DomainResult.Success();

        return DomainResult.Exception(
            $"Duplicate responsibilities found: {string.Join(", ", duplicates)}. " +
            "Each responsibility description must be unique.");
    }
}
