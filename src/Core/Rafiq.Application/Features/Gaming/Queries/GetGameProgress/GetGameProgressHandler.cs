using MediatR;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Features.Gaming.Queries.GetGameProgress;
public class GetGameProgressHandler(IGamingService gamingService) : IRequestHandler<GetGameProgressQuery, IReadOnlyList<GameProgressDto>>
{
    public async Task<IReadOnlyList<GameProgressDto>> Handle(GetGameProgressQuery request, CancellationToken cancellationToken)
    {
        return await gamingService.GetGameProgressAsync(request.GameId, request.PatientId);
    }
}
