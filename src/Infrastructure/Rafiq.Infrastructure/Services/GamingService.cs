using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services;

public class GamingService : IGamingService
{
    public Task<Result<IReadOnlyList<GamingOverviewCategoryDto>>> GetGamingOverviewAsync(int patientId)
    {
      
    }

    public Task<Result<SubmitGamePlayResponseDto>> SubmitGameResultAsync(SubmitGamePlayRequestDto dto)
    {
        throw new NotImplementedException();
    }
}