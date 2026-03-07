using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.AssessmentDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers;
public class AssessmentController(IAssessmentService assessmentService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> CreateAssessmentForPatientAsync(int patientId)
    {
        return (await assessmentService.CreateAssessmentForPatientAsync(patientId)).ToActionResult();
    }

    [HttpGet("assessment")]
    public async Task<IActionResult> GetAssessmentByPatientIdAsync(int assessmentId)
    {
        return (await assessmentService.GetAssessmentStatusAsync(assessmentId)).ToActionResult();
    }

    [HttpPost("answer")]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerDto dto)
    {
        var result = await assessmentService.SubmitAnswerAsync(dto);
        return Ok(result);
    }
}