using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers
{
    /// <summary>
    /// Handles the multi-step family registration process.
    /// </summary>
    /// <remarks>
    /// Provides endpoints for registering a family and patient information in multiple steps:
    /// 1. Submit family basic information (Step 1)
    /// 2. Submit patient details (Step 2)
    /// 3. Complete registration with password (Step 3)
    /// 4. Resume a registration session using a token
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class FamilyRegistrationController : BaseController
    {
        private readonly IFamilyRegistrationService registrationService;

        /// <summary>
        /// Initializes a new instance of <see cref="FamilyRegistrationController"/>.
        /// </summary>
        /// <param name="registrationService">Service that handles the family registration workflow.</param>
        public FamilyRegistrationController(IFamilyRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        /// <summary>
        /// Step 1: Submit family basic information.
        /// </summary>
        /// <param name="dto">The family information DTO containing Email, Relationship, Name, and EmergencyContact info.</param>
        /// <returns>
        /// Returns a token to continue the registration process and the next step number.
        /// </returns>
        /// <response code="200">Step 1 completed successfully, returns token and next step.</response>
        /// <response code="400">Validation failed (e.g., missing Email or invalid Relationship).</response>
        [HttpPost("step1")]
        [ProducesResponseType(typeof((string Token, int NextStep)), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Step1([FromBody] FamilyStep1Dto dto)
            => (await registrationService.RegisterFamilyStep1Async(dto)).ToActionResult();

        /// <summary>
        /// Step 2: Submit patient details.
        /// </summary>
        /// <param name="dto">The patient details DTO containing FirstName, LastName, DateOfBirth, Gender, and Token.</param>
        /// <returns>
        /// Returns the same token to continue the registration process and the next step number.
        /// </returns>
        /// <response code="200">Step 2 completed successfully, returns token and next step.</response>
        /// <response code="400">Invalid token or step sequence, or missing patient data.</response>
        /// <response code="404">Registration session not found or expired.</response>
        [HttpPost("step2")]
        [ProducesResponseType(typeof((string Token, int NextStep)), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Step2([FromBody] PatientStep2Dto dto)
            => (await registrationService.RegisterFamilyStep2Async(dto)).ToActionResult();

        /// <summary>
        /// Step 3: Complete family registration by setting a password.
        /// </summary>
        /// <param name="dto">The password DTO containing the registration token and password.</param>
        /// <returns>
        /// Returns a success message if registration is completed.
        /// </returns>
        /// <response code="200">Registration completed successfully.</response>
        /// <response code="400">Invalid token, incomplete registration data, or invalid step sequence.</response>
        /// <response code="404">Registration session not found or expired.</response>
        [HttpPost("step3")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Step3([FromBody] PasswordStepDto dto)
            => (await registrationService.CompleteFamilyRegistrationAsync(dto)).ToActionResult();

        /// <summary>
        /// Resume a previously started registration process using the token.
        /// </summary>
        /// <param name="token">The registration session token obtained from step 1 or 2.</param>
        /// <returns>
        /// Returns the cached registration session data.
        /// </returns>
        /// <response code="200">Registration session retrieved successfully.</response>
        /// <response code="400">Token is null or empty.</response>
        /// <response code="404">Registration session not found or expired.</response>
        [HttpGet("resume")]
        [ProducesResponseType(typeof(FamilyRegistrationCache), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Resume([FromQuery] string token)
            => (await registrationService.ResumeRegistrationAsync(token)).ToActionResult();
    }
}
