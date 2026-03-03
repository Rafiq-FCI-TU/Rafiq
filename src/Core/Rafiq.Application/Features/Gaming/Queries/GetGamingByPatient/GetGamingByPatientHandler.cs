using MediatR;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Features.Gaming.Queries.GetGamingByPatient;
public class GetGamingByPatientHandler(IGamingService gamingService) : IRequestHandler<GetGamingByPatientQuery, GamingDto>
{
    public async Task<GamingDto> Handle(GetGamingByPatientQuery request, CancellationToken cancellationToken)
    {
        return await gamingService.GetGamingByPatientAsync(request.PatientId);
    }
}
