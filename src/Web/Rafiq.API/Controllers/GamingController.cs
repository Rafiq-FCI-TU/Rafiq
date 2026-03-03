using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Features.Gaming.Commands.InitializeGaming;
using Rafiq.Application.Features.Gaming.Commands.RecordGameProgress;
using Rafiq.Application.Features.Gaming.Commands.UpdateGameStatus;
using Rafiq.Application.Features.Gaming.Queries.GetGameProgress;
using Rafiq.Application.Features.Gaming.Queries.GetGamesByCategory;
using Rafiq.Application.Features.Gaming.Queries.GetGamingByPatient;
using Rafiq.Domain.Enums;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Controllers;
public class GamingController(IMediator mediator) : BaseController
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("initialize/{patientId:int}")]
    public async Task<ActionResult> InitializeGaming([FromRoute] int patientId)
    {
        var result = await mediator.Send(new InitializeGamingCommand(patientId));
        return Result.Success(result, "Gaming initialized successfully").ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("patient/{patientId:int}")]
    public async Task<ActionResult> GetGamingByPatient([FromRoute] int patientId)
    {
        var result = await mediator.Send(new GetGamingByPatientQuery(patientId));
        return Result.Success(result, "Gaming data retrieved successfully").ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("category/{categoryId:int}/games")]
    public async Task<ActionResult> GetGamesByCategory([FromRoute] int categoryId)
    {
        var result = await mediator.Send(new GetGamesByCategoryQuery(categoryId));
        return Result.Success(result, "Games retrieved successfully").ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("progress")]
    public async Task<ActionResult> RecordGameProgress([FromBody] CreateGameProgressDto progress)
    {
        var result = await mediator.Send(new RecordGameProgressCommand(progress));
        return Result.Success(result, "Game progress recorded successfully").ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("progress/{gameId:int}/{patientId:int}")]
    public async Task<ActionResult> GetGameProgress([FromRoute] int gameId, [FromRoute] int patientId)
    {
        var result = await mediator.Send(new GetGameProgressQuery(gameId, patientId));
        return Result.Success(result, "Game progress retrieved successfully").ToActionResult();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("game/{gameId:int}/status")]
    public async Task<ActionResult> UpdateGameStatus([FromRoute] int gameId, [FromBody] UpdateGameStatusDto dto)
    {
        await mediator.Send(new UpdateGameStatusCommand(gameId, dto.Status));
        return Result.Success("Game status updated successfully").ToActionResult();
    }
}
