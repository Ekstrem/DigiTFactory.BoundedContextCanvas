namespace BoundedContextCanvas.InternalContracts;

public interface IBoundedContextCanvasQueryRepository
{
    Task<T?> GetByIdAsync<T>(Guid id, CancellationToken ct = default) where T : class;
    Task<IReadOnlyList<T>> QueryAsync<T>(Func<IQueryable<T>, IQueryable<T>>? filter = null, CancellationToken ct = default) where T : class;
    Task<T?> GetSingleAsync<T>(CancellationToken ct = default) where T : class;
}
