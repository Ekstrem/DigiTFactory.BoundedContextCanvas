using MediatR;
using BoundedContextCanvas.InternalContracts;

namespace BoundedContextCanvas.Application.Commands;

public sealed record CreateBoundedContextCommand(
    string TechnicalName,
    string OwnerTeam,
    string BusinessPurpose) : IRequest<BoundedContextCanvasOperationResult>;
