using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;

namespace BoundedContextCanvas.Domain.Specifications;

public class ResponsibilityCohesionValidator : IBusinessOperationValidator<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
{
    private string _reason = string.Empty;

    public string Reason => _reason;
    public DomainOperationResultEnum DomainResult => DomainOperationResultEnum.Exception;

    public bool IsSatisfiedBy(BusinessOperationData<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> obj)
    {
        var duplicates = obj.Model.Responsibilities
            .GroupBy(r => r.Description, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicates.Count == 0)
            return true;

        _reason = $"Duplicate responsibilities found: {string.Join(", ", duplicates)}. " +
                  "Each responsibility description must be unique.";
        return false;
    }
}
