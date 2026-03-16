using DigiTFactory.Libraries.SeedWorks.Invariants;
using DigiTFactory.Libraries.SeedWorks.Result;
using BoundedContextCanvas.Domain.Enums;

namespace BoundedContextCanvas.Domain.Specifications;

public class StatusGuardValidator : IBusinessOperationValidator<IBoundedContextCanvas, IBoundedContextCanvasAnemicModel>
{
    private readonly CanvasStatusEnum[] _allowedStatuses;
    private readonly string _commandName;

    public StatusGuardValidator(string commandName, params CanvasStatusEnum[] allowedStatuses)
    {
        _commandName = commandName;
        _allowedStatuses = allowedStatuses;
    }

    public DomainResult IsSatisfiedBy(IBoundedContextCanvasAnemicModel model)
    {
        if (_allowedStatuses.Contains(model.Root.Status))
            return DomainResult.Success();

        return DomainResult.Exception(
            $"Command '{_commandName}' is not allowed in status '{model.Root.Status}'. " +
            $"Allowed statuses: {string.Join(", ", _allowedStatuses)}");
    }
}
