using MediatR;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Features.Gaming.Commands.RecordGameProgress;
public class RecordGameProgressHandler(IGamingService gamingService) : IRequestHandler<RecordGameProgressCommand, GameProgressDto>
{
    public async Task<GameProgressDto> Handle(RecordGameProgressCommand request, CancellationToken cancellationToken)
    {
        return await gamingService.RecordGameProgressAsync(request.Progress);
    }
}
