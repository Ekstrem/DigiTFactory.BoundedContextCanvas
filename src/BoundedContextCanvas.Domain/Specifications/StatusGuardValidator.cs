using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Specifications;

public class StatusGuardValidator : IBusinessOperationValidator<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
{
    private readonly CanvasStatusEnum[] _allowedStatuses;
    private readonly string _commandName;
    private string _reason = string.Empty;

    public StatusGuardValidator(string commandName, params CanvasStatusEnum[] allowedStatuses)
    {
        _commandName = commandName;
        _allowedStatuses = allowedStatuses;
    }

    public string Reason => _reason;
    public DomainOperationResultEnum DomainResult => DomainOperationResultEnum.Exception;

    public bool IsSatisfiedBy(BusinessOperationData<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel> obj)
    {
        var model = obj.Model;
        if (_allowedStatuses.Contains(model.Root.Status))
            return true;

        _reason = $"Command '{_commandName}' is not allowed in status '{model.Root.Status}'. " +
                  $"Allowed statuses: {string.Join(", ", _allowedStatuses)}";
        return false;
    }
}
