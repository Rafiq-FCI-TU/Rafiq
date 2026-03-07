using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers;

/// <summary>
/// Manages game progress and score-based level unlocking for patients.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GamingController : BaseController
{
    private readonly IGamingService _gamingService;

    public GamingController(IGamingService gamingService)
    {
        _gamingService = gamingService;
    }

    #region Player Overview

    /// <summary>
    /// Gets the full gaming overview for a patient, including all categories,
    /// games, and levels with locked/unlocked status based on the patient's current score.
    /// </summary>
    /// <param name="patientId">The patient's ID.</param>
    /// <returns>Player gaming overview with level availability.</returns>
    /// <response code="200">Overview retrieved successfully.</response>
    /// <response code="404">Patient or gaming record not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("overview/{patientId}")]
    [ProducesResponseType(typeof(PlayerGamingOverviewDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetPlayerOverview(int patientId)
        => (await _gamingService.GetPlayerOverviewAsync(patientId)).ToActionResult();

    #endregion

    #region Submit Game Result

    /// <summary>
    /// Submits a game level completion result. Awards reward points and updates the patient's total score.
    /// </summary>
    /// <param name="patientId">The patient's ID.</param>
    /// <param name="dto">The game level completion data containing the gameLevelId.</param>
    /// <returns>Points earned, new total score, and newly unlocked levels.</returns>
    /// <response code="200">Game result submitted successfully.</response>
    /// <response code="400">Level is locked (insufficient score).</response>
    /// <response code="404">Patient or game level not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("submit/{patientId}")]
    [ProducesResponseType(typeof(GameResultResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> SubmitGameResult(int patientId, [FromBody] SubmitGameResultDto dto)
        => (await _gamingService.SubmitGameResultAsync(patientId, dto)).ToActionResult();

    #endregion
}
