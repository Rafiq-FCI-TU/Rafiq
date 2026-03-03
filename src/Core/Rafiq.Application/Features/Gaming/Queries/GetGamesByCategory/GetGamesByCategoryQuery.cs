using MediatR;
using Rafiq.Application.Dtos.GamingDtos;

namespace Rafiq.Application.Features.Gaming.Queries.GetGamesByCategory;
public record GetGamesByCategoryQuery(int CategoryId) : IRequest<IReadOnlyList<GameDto>>;
