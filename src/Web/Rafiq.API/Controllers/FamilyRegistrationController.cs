using Microsoft.AspNetCore.Mvc;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FamilyRegistrationController(IFamilyRegistrationService registrationService) : ControllerBase
{
    [HttpPost("step1")]
    public async Task<IActionResult> Step1([FromBody] FamilyStep1Dto dto)
    {
        var (token, nextStep) = await registrationService.SaveStep1Async(dto);
        return Ok(new { token, nextStep });
    }

    [HttpPost("step2")]
    public async Task<IActionResult> Step2([FromBody] PatientStep2Dto dto)
    {
        var nextStep = await registrationService.SaveStep2Async(dto);
        return Ok(new { nextStep });
    }

    [HttpPost("step3")]
    public async Task<IActionResult> Step3([FromBody] PasswordStep3Dto dto)
    {
        var success = await registrationService.CompleteRegistrationAsync(dto);
        return Ok(new { success });
    }

    [HttpGet("resume")]
    public async Task<IActionResult> Resume([FromQuery] string token)
    {
        var data = await registrationService.ResumeRegistrationAsync(token);
        if (data == null) return NotFound();
        return Ok(data);
    }
}