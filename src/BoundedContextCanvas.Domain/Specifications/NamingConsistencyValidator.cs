using System.Text.RegularExpressions;
using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;

namespace BoundedContextCanvas.Domain.Specifications;

public class NamingConsistencyValidator : IBusinessOperationValidator<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
{
    private static readonly Regex NamePattern = new("^[A-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);
    private string _reason = string.Empty;

    public string Reason => _reason;
    public DomainOperationResultEnum DomainResult => DomainOperationResultEnum.Exception;

    public bool IsSatisfiedBy(BusinessOperationData<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> obj)
    {
        var model = obj.Model;
        if (NamePattern.IsMatch(model.Root.TechnicalName))
            return true;

        _reason = $"Technical name '{model.Root.TechnicalName}' does not match naming convention. " +
                  "Must be PascalCase: ^[A-Z][a-zA-Z0-9]*$";
        return false;
    }
}
