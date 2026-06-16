using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Games;

public interface IPatientLevelService
{
    /// <summary>
    /// Persists a patient level result and returns the computed outcome.
    /// </summary>
    /// <param name="request">The patient level submission payload.</param>
    Task<Result<PatientLevelResultDto>> SubmitLevelResultAsync(SubmitPatientLevelRequest request);

    /// <summary>
    /// Retrieves a patient's aggregated game progress.
    /// </summary>
    /// <param name="patientId">The patient identifier.</param>
    Task<Result<PatientProgressSummaryDto>> GetPatientProgressAsync(int patientId);

    ///// <summary>
    ///// Retrieves the history of submissions for a patient and level.
    ///// </summary>
    ///// <param name="patientId">The patient identifier.</param>
    ///// <param name="gameLevelId">The game level identifier.</param>
    Task<Result<IReadOnlyList<PatientLevelResultDto>>> GetPatientLevelHistoryAsync(int patientId, int gameLevelId);
}
