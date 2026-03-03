using MediatR;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Features.Gaming.Commands.UpdateGameStatus;
public class UpdateGameStatusHandler(IGamingService gamingService) : IRequestHandler<UpdateGameStatusCommand>
{
    public async Task Handle(UpdateGameStatusCommand request, CancellationToken cancellationToken)
    {
        await gamingService.UpdateGameStatusAsync(request.GameId, request.Status);
    }
}
