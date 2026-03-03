using MediatR;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Features.Gaming.Commands.InitializeGaming;
public class InitializeGamingHandler(IGamingService gamingService) : IRequestHandler<InitializeGamingCommand, GamingDto>
{
    public async Task<GamingDto> Handle(InitializeGamingCommand request, CancellationToken cancellationToken)
    {
        return await gamingService.InitializeGamingAsync(request.PatientId);
    }
}
