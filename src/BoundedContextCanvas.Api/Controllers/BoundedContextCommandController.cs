using MediatR;
using Microsoft.AspNetCore.Mvc;
using BoundedContextCanvas.Api.Contracts.Requests;
using BoundedContextCanvas.Api.Contracts.Responses;
using BoundedContextCanvas.Application.Commands;

namespace BoundedContextCanvas.Api.Controllers;

/// <summary>Command API — изменение состояния Bounded Context Canvas.</summary>
[ApiController]
[Route("api/v1/bounded-context")]
[Produces("application/json")]
public class BoundedContextCommandController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoundedContextCommandController(IMediator mediator) => _mediator = mediator;

    /// <summary>Создать новый Bounded Context Canvas.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateBoundedContextRequest request, CancellationToken ct)
    {
        var command = new CreateBoundedContextCommand(request.TechnicalName, request.OwnerTeam, request.BusinessPurpose);
        var result = await _mediator.Send(command, ct);
        return result.Result == "Exception"
            ? BadRequest(ToResponse(result))
            : Ok(ToResponse(result));
    }

    /// <summary>Задать стратегическую классификацию.</summary>
    [HttpPost("{id:guid}/strategic-importance")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> DefineStrategicImportance(Guid id, [FromBody] DefineStrategicImportanceRequest request, CancellationToken ct)
    {
        var command = new DefineStrategicImportanceCommand(id, request.DomainType, request.BusinessModelRole, request.Evolution, request.RoleType, request.RoleDescription);
        var result = await _mediator.Send(command, ct);
        return result.Result == "Exception" ? BadRequest(ToResponse(result)) : Ok(ToResponse(result));
    }

    /// <summary>Определить границы контекста (ответственности).</summary>
    [HttpPost("{id:guid}/boundary")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RefineBoundary(Guid id, [FromBody] RefineBoundaryRequest request, CancellationToken ct)
    {
        var items = request.Responsibilities
            .Select(r => new RefineBoundaryCommand.ResponsibilityItem(r.Description, r.Type)).ToList();
        var command = new RefineBoundaryCommand(id, items);
        var result = await _mediator.Send(command, ct);
        return result.Result == "Exception" ? BadRequest(ToResponse(result)) : Ok(ToResponse(result));
    }

    /// <summary>Добавить термин в Ubiquitous Language.</summary>
    [HttpPost("{id:guid}/language/terms")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddTermToLanguage(Guid id, [FromBody] AddTermToLanguageRequest request, CancellationToken ct)
    {
        var command = new AddTermToLanguageCommand(id, request.Term, request.Definition);
        var result = await _mediator.Send(command, ct);
        return result.Result == "Exception" ? BadRequest(ToResponse(result)) : Ok(ToResponse(result));
    }

    /// <summary>Удалить термин из Ubiquitous Language.</summary>
    [HttpDelete("{id:guid}/language/terms/{term}")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    public async Task<IActionResult> RemoveTermFromLanguage(Guid id, string term, CancellationToken ct)
    {
        var command = new RemoveTermFromLanguageCommand(id, term);
        var result = await _mediator.Send(command, ct);
        return Ok(ToResponse(result));
    }

    /// <summary>Установить связь с другим контекстом.</summary>
    [HttpPost("{id:guid}/relationships")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> MapRelationship(Guid id, [FromBody] MapRelationshipRequest request, CancellationToken ct)
    {
        var command = new MapRelationshipCommand(id, request.TargetContextId, request.TargetContextName, request.Direction, request.Pattern);
        var result = await _mediator.Send(command, ct);
        return result.Result == "Exception" ? BadRequest(ToResponse(result)) : Ok(ToResponse(result));
    }

    /// <summary>Удалить связь с контекстом.</summary>
    [HttpDelete("{id:guid}/relationships/{targetId:guid}")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    public async Task<IActionResult> RemoveRelationship(Guid id, Guid targetId, CancellationToken ct)
    {
        var command = new RemoveRelationshipCommand(id, targetId);
        var result = await _mediator.Send(command, ct);
        return Ok(ToResponse(result));
    }

    /// <summary>Опубликовать контракт (публичный интерфейс).</summary>
    [HttpPost("{id:guid}/contract")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PublishContract(Guid id, [FromBody] PublishContractRequest request, CancellationToken ct)
    {
        var items = request.Items
            .Select(i => new PublishContractCommand.InterfaceItem(i.Name, i.Type, i.Direction, i.LinkedResponsibility)).ToList();
        var command = new PublishContractCommand(id, items);
        var result = await _mediator.Send(command, ct);
        return result.Result == "Exception" ? BadRequest(ToResponse(result)) : Ok(ToResponse(result));
    }

    /// <summary>Добавить бизнес-решение.</summary>
    [HttpPost("{id:guid}/business-decisions")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddBusinessDecision(Guid id, [FromBody] AddBusinessDecisionRequest request, CancellationToken ct)
    {
        var command = new AddBusinessDecisionCommand(id, request.Rule, request.Rationale);
        var result = await _mediator.Send(command, ct);
        return result.Result == "Exception" ? BadRequest(ToResponse(result)) : Ok(ToResponse(result));
    }

    /// <summary>Добавить предположение.</summary>
    [HttpPost("{id:guid}/assumptions")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddAssumption(Guid id, [FromBody] AddAssumptionRequest request, CancellationToken ct)
    {
        var command = new AddAssumptionCommand(id, request.Statement, request.Risk);
        var result = await _mediator.Send(command, ct);
        return result.Result == "Exception" ? BadRequest(ToResponse(result)) : Ok(ToResponse(result));
    }

    /// <summary>Архивировать канвас.</summary>
    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(typeof(BoundedContextCanvasOperationResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)
    {
        var command = new ArchiveBoundedContextCommand(id);
        var result = await _mediator.Send(command, ct);
        return result.Result == "Exception" ? BadRequest(ToResponse(result)) : Ok(ToResponse(result));
    }

    private static BoundedContextCanvasOperationResponse ToResponse(InternalContracts.BoundedContextCanvasOperationResult r)
        => new(r.AggregateId, r.Version, r.Result, r.Reasons);
}
