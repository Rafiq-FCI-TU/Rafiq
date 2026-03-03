using MediatR;
using Rafiq.Application.Dtos.GamingDtos;

namespace Rafiq.Application.Features.Gaming.Commands.InitializeGaming;
public record InitializeGamingCommand(int PatientId) : IRequest<GamingDto>;
