using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.SessionDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Controllers
{
    /// <summary>
    /// Handles session management for specialists and patients.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : BaseController
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        #region Create Session

        /// <summary>
        /// Creates a new session with optional video and thumbnail uploads.
        /// </summary>
        /// <param name="dto">Session creation DTO including media files.</param>
        /// <returns>Returns the ID of the newly created session.</returns>
        /// <response code="200">Session created successfully.</response>
        /// <response code="422">Media or thumbnail upload failed.</response>
        /// <response code="500">Internal server error during creation.</response>
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Create([FromForm] SessionCreateDto dto)
        {
            var result = await _sessionService.CreateSessionAsync(dto);
            return result.ToActionResult();
        }

        #endregion

        #region Get Sessions

        /// <summary>
        /// Gets session details by session ID.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>Session details DTO.</returns>
        /// <response code="200">Session found.</response>
        /// <response code="404">Session not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("{sessionId}")]
        [ProducesResponseType(typeof(SessionDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSessionDetails(int sessionId)
            => (await _sessionService.GetSessionDetailsAsync(sessionId)).ToActionResult();

        /// <summary>
        /// Gets all sessions for a specific specialist.
        /// </summary>
        /// <param name="specialistId">Specialist profile ID.</param>
        /// <returns>List of sessions for that specialist.</returns>
        [HttpGet("specialist/{specialistId}")]
        [ProducesResponseType(typeof(IEnumerable<SessionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSessionsBySpecialist(string specialistId)
            => (await _sessionService.GetSessionsBySpecialistAsync(specialistId)).ToActionResult();

        /// <summary>
        /// Gets all sessions for a specialist filtered by patient.
        /// </summary>
        /// <param name="specialistId">Specialist profile ID.</param>
        /// <param name="patientId">Patient ID.</param>
        /// <returns>List of sessions accessible to the patient.</returns>
        [HttpGet("specialist/{specialistId}/patient/{patientId}")]
        [ProducesResponseType(typeof(IEnumerable<SessionDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSessionsBySpecialistForPatient(string specialistId, int patientId)
            => (await _sessionService.GetSessionsBySpecialistIdForPatientAsync(specialistId, patientId)).ToActionResult();

        /// <summary>
        /// Gets the video URL for a specific session and patient.
        /// </summary>
        /// <param name="sessionId">Session ID.</param>
        /// <param name="patientId">Patient ID.</param>
        /// <returns>Video URL of the session.</returns>
        [HttpGet("{sessionId}/video/{patientId}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSessionVideoUrl(int sessionId, int patientId)
            => (await _sessionService.GetSessionVideoUrlAsync(sessionId, patientId)).ToActionResult();

        #endregion
    }
}