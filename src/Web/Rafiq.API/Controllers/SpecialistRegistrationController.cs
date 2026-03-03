using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.StepsDtos;

namespace Rafiq.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpecialistRegistrationController(ISpecialistRegistrationService registrationService) : BaseController
{
    [HttpPost("step1")]
    [ProducesResponseType(typeof((string Token, int NextStep)), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Step1([FromBody] SpecialistStep1Dto dto) => (await registrationService.RegisterSpecialistStep1Async(dto)).ToActionResult();

    [HttpPost("step2")]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Step3([FromBody] PasswordStepDto dto) => (await registrationService.CompleteSpecialistRegistrationAsync(dto)).ToActionResult();

    [HttpGet("resume")]
    [ProducesResponseType(typeof(SpecialistRegistrationCache), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Resume([FromQuery] string token) => (await registrationService.ResumeRegistrationAsync(token)).ToActionResult();
}
