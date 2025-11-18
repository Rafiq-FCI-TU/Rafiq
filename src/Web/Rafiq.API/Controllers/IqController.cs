using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rafiq.Application.Features.IQ.Commands.StartAssessment;
using Rafiq.Application.Features.IQ.Commands.SaveAnswers;
using Rafiq.Application.Features.IQ.Commands.DeleteAssessment;
using Rafiq.Application.Features.IQ.Queries.GetQuestions;
using Rafiq.Application.Features.IQ.Queries.GetAssessmentProgress;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.IQDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Controllers;
public class IqController(IMediator mediator) : BaseController
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("start")]
    public async Task<ActionResult> StartAssessment([FromRoute] int patientId)
    {
        var result = await mediator.Send(new StartAssessmentCommand(patientId));
        return Result.Success(result, "Assessment started successfully").ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("questions")]
    public async Task<ActionResult> GetQuestions()
    {
        var result = await mediator.Send(new GetQuestionsQuery());
        return Result.Success(result, "Questions retrieved successfully").ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("{assessmentId:int}/answers")]
    public async Task<ActionResult> SaveAnswers([FromRoute] int assessmentId, [FromBody] IReadOnlyList<AnswerDto> answers)
    {
        await mediator.Send(new SaveAnswersCommand(assessmentId, answers));
        return Result.Success("Answers saved successfully").ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("progress")]
    public async Task<ActionResult> GetProgress([FromRoute] int patientId)
    {
        var result = await mediator.Send(new GetAssessmentProgressQuery(patientId));
        return Result.Success(result, "Progress retrieved successfully").ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{assessmentId:int}")]
    public async Task<ActionResult> DeleteAssessment([FromRoute] int assessmentId)
    {
        await mediator.Send(new DeleteAssessmentCommand(assessmentId));
        return Result.Success("Assessment deleted successfully").ToActionResult();
    }
}
