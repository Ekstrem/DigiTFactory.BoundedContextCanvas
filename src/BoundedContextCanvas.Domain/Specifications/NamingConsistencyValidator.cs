using System.Text.RegularExpressions;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;

namespace BoundedContextCanvas.Domain.Specifications;

public class NamingConsistencyValidator : IBusinessOperationValidator<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
{
    private static readonly Regex NamePattern = new("^[A-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);

    public DomainResult IsSatisfiedBy(IBoundedContextCanvasAnemicModel model)
    {
        if (NamePattern.IsMatch(model.Root.TechnicalName))
            return DomainResult.Success();

        return DomainResult.Exception(
            $"Technical name '{model.Root.TechnicalName}' does not match naming convention. " +
            "Must be PascalCase: ^[A-Z][a-zA-Z0-9]*$");
    }
}
