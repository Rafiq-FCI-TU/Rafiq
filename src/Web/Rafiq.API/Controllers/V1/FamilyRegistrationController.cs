using Microsoft.AspNetCore.Mvc;
using Rafiq.Application.Dtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers.V1;

[Route("api/[controller]")]
[ApiController]
public class FamilyRegistrationController : ControllerBase
{
    private readonly IFamilyRegistrationService _registrationService;

    public FamilyRegistrationController(IFamilyRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpPost("step1")]
    public async Task<IActionResult> Step1([FromBody] FamilyStep1Dto dto)
    {
        var (token, nextStep) = await _registrationService.SaveStep1Async(dto);
        return Ok(new { token, nextStep });
    }

    [HttpPost("step2")]
    public async Task<IActionResult> Step2([FromBody] PatientStep2Dto dto)
    {
        var nextStep = await _registrationService.SaveStep2Async(dto);
        return Ok(new { nextStep });
    }

    [HttpPost("step3")]
    public async Task<IActionResult> Step3([FromBody] PasswordStep3Dto dto)
    {
        var success = await _registrationService.CompleteRegistrationAsync(dto);
        return Ok(new { success });
    }

    [HttpGet("resume")]
    public async Task<IActionResult> Resume([FromQuery] string token)
    {
        var data = await _registrationService.ResumeRegistrationAsync(token);
        if (data == null) return NotFound();
        return Ok(data);
    }
}