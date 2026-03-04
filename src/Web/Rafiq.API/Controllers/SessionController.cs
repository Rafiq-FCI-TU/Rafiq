using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.SessionDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;
using System.Net.NetworkInformation;

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
        [HttpGet("SessionDetails/{sessionId}")]
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
        /// Retrieves sessions for a patient filtered by status.
        /// </summary>
        /// <param name="patientId">The unique ID of the patient.</param>
        /// <param name="status">
        /// Optional query parameter to filter sessions:
        /// <c>valid</c> - sessions that match the patient's total score level.
        /// <c>notvalid</c> - sessions above the patient's total score level.
        /// If not specified, all sessions for the patient will be returned.
        /// </param>
        /// <returns>
        /// Returns a list of <see cref="Sessionfor_Specialist_patientDto"/> representing
        /// the sessions that match the given criteria for the patient.
        /// </returns>
        /// <response code="200">Sessions found matching the status filter.</response>
        /// <response code="404">No sessions found for the patient or matching the specified status.</response>
        /// <response code="400">Invalid status value provided.</response>
        /// <response code="500">An unexpected server error occurred while retrieving sessions.</response>
        [HttpGet("patient/{patientId}/sessions")]
        [ProducesResponseType(typeof(IEnumerable<Sessionfor_Specialist_patientDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetSessionsForPatientByStatus(
            int patientId,
            [FromQuery] string? status= "valid")
            => (await _sessionService.GetSessionsByPatientAndStatusAsync(patientId, status)).ToActionResult();

        #endregion
    }
}