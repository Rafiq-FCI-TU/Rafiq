using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers;

[ApiController]
[Route("api/gaming")]
public class GamingController : BaseController
{
    private readonly IGamingService _gamingService;

    public GamingController(IGamingService gamingService)
    {
        _gamingService = gamingService;
    }

    [HttpGet("overview/{patientId}")]
    [ProducesResponseType(typeof(IReadOnlyList<GamingOverviewCategoryDto>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetOverview(int patientId)
        => (await _gamingService.GetGamingOverviewAsync(patientId)).ToActionResult();

    [HttpPost("play")]
    [ProducesResponseType(typeof(SubmitGamePlayResponseDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Play([FromBody] SubmitGamePlayRequestDto dto)
        => (await _gamingService.SubmitGameResultAsync(dto)).ToActionResult();
}
