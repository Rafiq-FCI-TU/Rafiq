using Microsoft.AspNetCore.Mvc;
using Rafiq.Application.Games;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Controllers;

[ApiController]
[Route("api/game-levels")]
public class GameLevelsController : ControllerBase
{
    private readonly IGameLevelService _gameLevelService;

    public GameLevelsController(IGameLevelService gameLevelService)
    {
        _gameLevelService = gameLevelService;
    }

    /// <summary>
    /// Retrieves all levels for a game.
    /// </summary>
    /// <param name="gameId">The game identifier.</param>
    [HttpGet("game/{gameId}")]
    public async Task<IActionResult> GetLevelsByGameAsync(int gameId)
    {
        var result = await _gameLevelService.GetLevelsByGameAsync(gameId);
        return HandleResult(result);
    }

    /// <summary>
    /// Retrieves a level with its questions.
    /// </summary>
    /// <param name="levelId">The level identifier.</param>
    [HttpGet("{levelId}/questions")]
    public async Task<IActionResult> GetLevelWithQuestionsAsync(int levelId)
    {
        var result = await _gameLevelService.GetLevelWithQuestionsAsync(levelId);
        return HandleResult(result);
    }

    private IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(),
            _ => StatusCode(result.StatusCode, result)
        };
    }
}
