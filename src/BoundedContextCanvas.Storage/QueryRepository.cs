using Microsoft.EntityFrameworkCore;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Storage;

public class QueryRepository : IBoundedContextCanvasQueryRepository
{
    private readonly ReadDbContext _context;

    public QueryRepository(ReadDbContext context) => _context = context;

    public async Task<T?> GetByIdAsync<T>(Guid id, CancellationToken ct = default) where T : class
    {
        return await _context.Set<T>().FindAsync(new object[] { id }, ct);
    }

    public async Task<IReadOnlyList<T>> QueryAsync<T>(Func<IQueryable<T>, IQueryable<T>>? filter = null, CancellationToken ct = default) where T : class
    {
        IQueryable<T> query = _context.Set<T>();
        if (filter is not null)
            query = filter(query);
        return await query.ToListAsync(ct);
    }

    public async Task<T?> GetSingleAsync<T>(CancellationToken ct = default) where T : class
    {
        return await _context.Set<T>().FirstOrDefaultAsync(ct);
    }
}
