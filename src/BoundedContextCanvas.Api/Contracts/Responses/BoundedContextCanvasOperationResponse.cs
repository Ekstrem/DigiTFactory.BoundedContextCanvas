namespace BoundedContextCanvas.Api.Contracts.Responses;

/// <summary>Единый ответ для всех команд.</summary>
public sealed record BoundedContextCanvasOperationResponse(
    Guid AggregateId,
    long Version,
    string Result,
    IReadOnlyList<string>? Reasons);
