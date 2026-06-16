using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers;

/// <summary>
/// Handles specialist management and patient assignment operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SpecialistController : BaseController
{
    private readonly ISpecialistService _specialistService;

    public SpecialistController(ISpecialistService specialistService)
    {
        _specialistService = specialistService;
    }

    #region Get Specialists

    /// <summary>
    /// Gets all registered specialists.
    /// </summary>
    /// <returns>List of all specialists.</returns>
    /// <response code="200">Specialists retrieved successfully.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SpecialistResponseDto>), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAllSpecialists()
        => (await _specialistService.GetAllSpecialistsAsync()).ToActionResult();

    /// <summary>
    /// Gets a specific specialist by ID.
    /// </summary>
    /// <param name="id">The specialist profile ID.</param>
    /// <returns>Specialist details.</returns>
    /// <response code="200">Specialist found.</response>
    /// <response code="404">Specialist not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SpecialistResponseDto), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetSpecialistById(string id)
        => (await _specialistService.GetSpecialistByIdAsync(id)).ToActionResult();

    /// <summary>
    /// Gets all patients assigned to a specific specialist.
    /// </summary>
    /// <param name="id">The specialist profile ID.</param>
    /// <returns>List of patients assigned to the specialist.</returns>
    /// <response code="200">Patients retrieved successfully.</response>
    /// <response code="404">Specialist not found or has no patients.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("{id}/patients")]
    [ProducesResponseType(typeof(IEnumerable<SpecialistPatientDto>), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetSpecialistPatients(string id)
        => (await _specialistService.GetSpecialistPatientsAsync(id)).ToActionResult();

    #endregion

    #region Assign / Unassign Patient

    /// <summary>
    /// Assigns a patient to a specialist.
    /// </summary>
    /// <param name="specialistId">The specialist profile ID.</param>
    /// <param name="patientId">The patient ID.</param>
    /// <returns>Success message if assigned.</returns>
    /// <response code="200">Patient assigned successfully.</response>
    /// <response code="409">Patient is already assigned to this specialist.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("{specialistId}/patients/{patientId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(409)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> AssignPatient(string specialistId, int patientId)
        => (await _specialistService.AssignPatientToSpecialistAsync(specialistId, patientId)).ToActionResult();

    /// <summary>
    /// Unassigns a patient from a specialist.
    /// </summary>
    /// <param name="specialistId">The specialist profile ID.</param>
    /// <param name="patientId">The patient ID.</param>
    /// <returns>Success message if unassigned.</returns>
    /// <response code="200">Patient unassigned successfully.</response>
    /// <response code="404">Patient is not assigned to this specialist.</response>
    /// <response code="500">Internal server error.</response>
    [HttpDelete("{specialistId}/patients/{patientId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UnassignPatient(string specialistId, int patientId)
        => (await _specialistService.UnassignPatientFromSpecialistAsync(specialistId, patientId)).ToActionResult();

    #endregion
}