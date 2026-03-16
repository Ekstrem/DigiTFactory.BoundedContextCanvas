using System.ComponentModel.DataAnnotations;

namespace BoundedContextCanvas.Api.Contracts.Requests;

/// <summary>Публикация контракта (публичного интерфейса) контекста.</summary>
public sealed record PublishContractRequest
{
    /// <summary>Элементы публичного интерфейса.</summary>
    [Required] public List<PublicInterfaceItemRequest> Items { get; init; } = new();
}

/// <summary>Элемент публичного интерфейса.</summary>
public sealed record PublicInterfaceItemRequest
{
    /// <summary>Имя элемента интерфейса.</summary>
    [Required] public string Name { get; init; } = default!;
    /// <summary>Command | Event | Query</summary>
    [Required] public string Type { get; init; } = default!;
    /// <summary>Inbound | Outbound</summary>
    [Required] public string Direction { get; init; } = default!;
    /// <summary>Связанная ответственность контекста.</summary>
    [Required] public string LinkedResponsibility { get; init; } = default!;
}
