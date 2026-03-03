using MediatR;
using Rafiq.Domain.Enums;

namespace Rafiq.Application.Features.Gaming.Commands.UpdateGameStatus;
public record UpdateGameStatusCommand(int GameId, GameStatus Status) : IRequest;
