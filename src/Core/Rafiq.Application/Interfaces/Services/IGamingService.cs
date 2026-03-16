using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;

public interface IGamingService
{
    Task<Result<IReadOnlyList<GamingOverviewCategoryDto>>> GetGamingOverviewAsync(int patientId);

    Task<Result<SubmitGamePlayResponseDto>> SubmitGameResultAsync(SubmitGamePlayRequestDto dto);
}
