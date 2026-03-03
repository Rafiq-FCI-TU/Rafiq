using Rafiq.Application.Dtos.SessionDtos;
using Rafiq.Domain.ErrorHandling;
using System.Collections.Generic;


namespace Rafiq.Application.Interfaces.Services
{
    public interface ISessionService
    {
        // ---------------- Specialist ----------------
        #region Specialist 

        /// <summary>
        /// Creates a new session for a specialist.
        /// </summary>
        /// <param name="dto">Session creation data</param>
        /// <returns>Id of the created session</returns>
        Task<Result<int>> CreateSessionAsync(SessionCreateDto dto);

        /// <summary>
        /// Get all sessions for a specialist
        /// </summary>
        /// <param name="specialistId">Specialist identifier</param>
        /// <returns>List of sessions</returns>
        Task<Result<IEnumerable<SessionDto>>> GetSessionsBySpecialistAsync(string specialistId);

        #endregion

        // ---------------- Patient ----------------
        #region Patient

        /// <summary>
        /// Get all sessions for a specific patient under a specific specialist
        /// Filter sessions based on patient score or session score
        /// </summary>
        /// <param name="patientId">Patient identifier</param>
        /// <returns>List of sessions available to the patient</returns>
        Task<Result<IEnumerable<SessionDto>>> GetSessionsBySpecialistIdForPatientAsync(string specialistId, int patientId);

        #endregion

        // ---------------- Session ----------------
        /// <summary>
        /// Get video URL for a patient to access a session recording.
        /// Verify patient access using patientId and sessionId.
        /// </summary>
        Task<Result<string>> GetSessionVideoUrlAsync(int sessionId, int patientId);

        /// <summary>
        /// Get details of a single session by sessionId
        /// </summary>
        Task<Result<SessionDto>> GetSessionDetailsAsync(int sessionId);
    }
}