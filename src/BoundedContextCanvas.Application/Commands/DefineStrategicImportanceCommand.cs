using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record DefineStrategicImportanceCommand(
    Guid AggregateId,
    string DomainType,
    string BusinessModelRole,
    string Evolution,
    string RoleType,
    string RoleDescription) : IRequest<BoundedContextCanvasOperationResult>;
