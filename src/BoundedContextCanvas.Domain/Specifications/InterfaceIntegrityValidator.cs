using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Specifications;

public class InterfaceIntegrityValidator : IBusinessOperationValidator<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
{
    private string _reason = string.Empty;

    public string Reason => _reason;
    public DomainOperationResultEnum DomainResult => DomainOperationResultEnum.Exception;

    public bool IsSatisfiedBy(BusinessOperationData<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> obj)
    {
        var model = obj.Model;
        var outboundEvents = model.PublicInterface
            .Where(i => i.Direction == InterfaceDirectionEnum.Outbound && i.Type == InterfaceItemTypeEnum.DomainEvent)
            .ToList();

        var responsibilityDescriptions = model.Responsibilities
            .Select(r => r.Description)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var orphaned = outboundEvents
            .Where(e => string.IsNullOrWhiteSpace(e.LinkedResponsibility) ||
                        !responsibilityDescriptions.Contains(e.LinkedResponsibility))
            .Select(e => e.Name)
            .ToList();

        if (orphaned.Count == 0)
            return true;

        _reason = $"Outbound domain events must be linked to a responsibility. " +
                  $"Orphaned events: {string.Join(", ", orphaned)}";
        return false;
    }
}
