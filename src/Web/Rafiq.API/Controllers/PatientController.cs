using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.PatientDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : BaseController
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PatientResponseDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllPatients()
        => (await _patientService.GetAllPatientsAsync()).ToActionResult();

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PatientResponseDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetPatientById(int id)
        => (await _patientService.GetPatientByIdAsync(id)).ToActionResult();

    [HttpGet("{id}/doctor")]
    [ProducesResponseType(typeof(PatientDoctorDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetPatientDoctor(int id)
        => (await _patientService.GetPatientDoctorAsync(id)).ToActionResult();
}
