using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;

public interface IGamingService
{
    /// <summary>
    /// Gets the full gaming overview for a patient, including all categories, games,
    /// and levels with locked/unlocked status based on the patient's current score.
    /// </summary>
    Task<Result<PlayerGamingOverviewDto>> GetPlayerOverviewAsync(int patientId);

    /// <summary>
    /// Submits a game level completion result. Validates the level is unlocked,
    /// records the progress, and updates the patient's total score.
    /// </summary>
    Task<Result<GameResultResponseDto>> SubmitGameResultAsync(int patientId, SubmitGameResultDto dto);
}
