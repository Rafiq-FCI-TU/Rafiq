using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers
{
    /// <summary>
    /// Handles operations related to patient session attempts.
    /// Allows patients to submit attempts and specialists to review them.
    /// </summary>
    public class PatientAttemptsController(IPatientSessionAttemptService _attemptService) : BaseController
    {
        /// <summary>
        /// Submit a new attempt for a session by uploading a video.
        /// </summary>
        /// <param name="dto">Attempt submission data including session, patient, and video file.</param>
        /// <returns>The ID of the created attempt.</returns>
        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromForm] SubmitAttemptDto dto)
            => (await _attemptService.SubmitAttemptAsync(dto)).ToActionResult();


        /// <summary>
        /// Review a patient's attempt by approving or rejecting it.
        /// Specialists can also provide feedback and assign a score.
        /// </summary>
        /// <param name="dto">Review details including status, feedback, and optional score.</param>
        /// <returns>True if the review was completed successfully.</returns>
        [HttpPut("review")]
        public async Task<IActionResult> Review([FromBody] ReviewAttemptDto dto)
             => (await _attemptService.ReviewAttemptAsync(dto)).ToActionResult();


        /// <summary>
        /// Get all pending attempts that require review by a specific specialist.
        /// </summary>
        /// <param name="specialistId">The specialist profile identifier.</param>
        /// <returns>List of attempts waiting for review.</returns>
        [HttpGet("pending/{specialistId}")]
        public async Task<IActionResult> GetPending(string specialistId)
             => (await _attemptService.GetPendingAttemptsForSpecialistAsync(specialistId)).ToActionResult();


        /// <summary>
        /// Retrieve all attempts submitted by a specific patient.
        /// </summary>
        /// <param name="patientId">The patient identifier.</param>
        /// <returns>List of attempts submitted by the patient.</returns>
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
             => (await _attemptService.GetAttemptsByPatientAsync(patientId)).ToActionResult();
    }
}
