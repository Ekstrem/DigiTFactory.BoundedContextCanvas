namespace BoundedContextCanvas.InternalContracts;

public sealed class BoundedContextCanvasOperationResult
{
    public Guid AggregateId { get; }
    public long Version { get; }
    public string Result { get; }
    public IReadOnlyList<string>? Reasons { get; }

    private BoundedContextCanvasOperationResult(Guid aggregateId, long version, string result, IReadOnlyList<string>? reasons)
    {
        AggregateId = aggregateId;
        Version = version;
        Result = result;
        Reasons = reasons;
    }

    public static BoundedContextCanvasOperationResult Success(Guid aggregateId, long version)
        => new(aggregateId, version, "Success", null);

    public static BoundedContextCanvasOperationResult WithWarnings(Guid aggregateId, long version, IReadOnlyList<string> reasons)
        => new(aggregateId, version, "WithWarnings", reasons);

    public static BoundedContextCanvasOperationResult Exception(Guid aggregateId, long version, IReadOnlyList<string> reasons)
        => new(aggregateId, version, "Exception", reasons);
}
