using MediatR;
using Microsoft.AspNetCore.Mvc;
using BoundedContextCanvas.Application.Queries;

namespace BoundedContextCanvas.Api.Controllers;

/// <summary>Query API — чтение данных Bounded Context Canvas из Read Store.</summary>
[ApiController]
[Route("api/v1/bounded-context")]
[Produces("application/json")]
public class BoundedContextQueryController : ControllerBase
{
    private readonly IMediator _mediator;

    public BoundedContextQueryController(IMediator mediator) => _mediator = mediator;

    /// <summary>Получить детали канваса по ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BoundedContextDetailResult), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetDetail(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetBoundedContextDetailQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Получить список канвасов с фильтрами.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(BoundedContextListResult), 200)]
    public async Task<IActionResult> GetList(
        [FromQuery] string? ownerTeam,
        [FromQuery] string? domainType,
        [FromQuery] string? status,
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string sortBy = "updatedAt",
        [FromQuery] string sortDir = "desc",
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(
            new GetBoundedContextListQuery(ownerTeam, domainType, status, search, page, pageSize, sortBy, sortDir), ct);
        return Ok(result);
    }

    /// <summary>Получить граф связей (Context Map).</summary>
    [HttpGet("relationships/graph")]
    [ProducesResponseType(typeof(ContextMapGraphResult), 200)]
    public async Task<IActionResult> GetContextMapGraph(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetContextMapGraphQuery(), ct);
        return Ok(result);
    }

    /// <summary>Получить статистику контекстов.</summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(ContextStatsResult), 200)]
    public async Task<IActionResult> GetStats(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetContextStatsQuery(), ct);
        return Ok(result);
    }
}
