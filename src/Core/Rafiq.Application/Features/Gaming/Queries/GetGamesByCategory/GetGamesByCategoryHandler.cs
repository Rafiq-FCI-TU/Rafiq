using MediatR;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Features.Gaming.Queries.GetGamesByCategory;
public class GetGamesByCategoryHandler(IGamingService gamingService) : IRequestHandler<GetGamesByCategoryQuery, IReadOnlyList<GameDto>>
{
    public async Task<IReadOnlyList<GameDto>> Handle(GetGamesByCategoryQuery request, CancellationToken cancellationToken)
    {
        return await gamingService.GetGamesByCategoryAsync(request.CategoryId);
    }
}
