using Microsoft.AspNetCore.Mvc;
using Rafiq.Application.Games;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Controllers;

[ApiController]
[Route("api/patient-progress")]
public class PatientProgressController : ControllerBase
{
    private readonly IPatientLevelService _patientLevelService;

    public PatientProgressController(IPatientLevelService patientLevelService)
    {
        _patientLevelService = patientLevelService;
    }

    /// <summary>
    /// Submits a patient level result.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SubmitLevelResultAsync([FromBody] SubmitPatientLevelRequest request)
    {
        var result = await _patientLevelService.SubmitLevelResultAsync(request);
        return HandleResult(result);
    }

    /// <summary>
    /// Retrieves the history of submissions for a patient and level.
    /// </summary>
    [HttpGet("{patientId}/levels/{gameLevelId}/history")]
    public async Task<IActionResult> GetPatientLevelHistoryAsync(int patientId, int gameLevelId)
    {
        var result = await _patientLevelService.GetPatientLevelHistoryAsync(patientId, gameLevelId);
        return HandleResult(result);
    }

    /// <summary>
    /// Retrieves a patient's aggregated game progress.
    /// </summary>
    [HttpGet("{patientId}/progress")]
    public async Task<IActionResult> GetPatientProgressAsync(int patientId)
    {
        var result = await _patientLevelService.GetPatientProgressAsync(patientId);
        return HandleResult(result);
    }

    private IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result);

        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(result),
            _ => StatusCode(result.StatusCode, result)
        };
    }
}