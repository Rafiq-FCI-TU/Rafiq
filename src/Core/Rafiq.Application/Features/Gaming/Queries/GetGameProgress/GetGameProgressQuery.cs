using MediatR;
using Rafiq.Application.Dtos.GamingDtos;

namespace Rafiq.Application.Features.Gaming.Queries.GetGameProgress;
public record GetGameProgressQuery(int GameId, int PatientId) : IRequest<IReadOnlyList<GameProgressDto>>;
