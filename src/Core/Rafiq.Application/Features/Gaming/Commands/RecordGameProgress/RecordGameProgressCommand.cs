using MediatR;
using Rafiq.Application.Dtos.GamingDtos;

namespace Rafiq.Application.Features.Gaming.Commands.RecordGameProgress;
public record RecordGameProgressCommand(CreateGameProgressDto Progress) : IRequest<GameProgressDto>;
