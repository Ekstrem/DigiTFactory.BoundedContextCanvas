using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Queries;

public sealed class GetBoundedContextListHandler : IRequestHandler<GetBoundedContextListQuery, BoundedContextListResult>
{
    private readonly IBoundedContextCanvasQueryRepository _repository;

    public GetBoundedContextListHandler(IBoundedContextCanvasQueryRepository repository)
        => _repository = repository;

    public async Task<BoundedContextListResult> Handle(GetBoundedContextListQuery request, CancellationToken ct)
    {
        var items = await _repository.QueryAsync<BoundedContextListItemResult>(ct: ct);

        var filtered = items.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.OwnerTeam))
            filtered = filtered.Where(x => x.OwnerTeam == request.OwnerTeam);

        if (!string.IsNullOrWhiteSpace(request.DomainType))
            filtered = filtered.Where(x => x.DomainType == request.DomainType);

        if (!string.IsNullOrWhiteSpace(request.Status))
            filtered = filtered.Where(x => x.Status == request.Status);

        if (!string.IsNullOrWhiteSpace(request.Search))
            filtered = filtered.Where(x =>
                x.TechnicalName.Contains(request.Search, StringComparison.OrdinalIgnoreCase) ||
                (x.BusinessPurpose != null && x.BusinessPurpose.Contains(request.Search, StringComparison.OrdinalIgnoreCase)));

        var total = filtered.Count();

        filtered = request.SortBy.ToLowerInvariant() switch
        {
            "technicalname" => request.SortDir == "asc" ? filtered.OrderBy(x => x.TechnicalName) : filtered.OrderByDescending(x => x.TechnicalName),
            "ownerteam" => request.SortDir == "asc" ? filtered.OrderBy(x => x.OwnerTeam) : filtered.OrderByDescending(x => x.OwnerTeam),
            "status" => request.SortDir == "asc" ? filtered.OrderBy(x => x.Status) : filtered.OrderByDescending(x => x.Status),
            _ => request.SortDir == "asc" ? filtered.OrderBy(x => x.UpdatedAt) : filtered.OrderByDescending(x => x.UpdatedAt),
        };

        var paged = filtered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new BoundedContextListResult(paged, total, request.Page, request.PageSize);
    }
}
