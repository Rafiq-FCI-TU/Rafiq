using Microsoft.AspNetCore.Mvc;
using Rafiq.Application.Games;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.API.Controllers;

[ApiController]
[Route("api/games")]
public class GamesController : ControllerBase
{
    private readonly IGameService _gameService;

    public GamesController(IGameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Retrieves all games or filters them by category.
    /// </summary>
    /// <param name="categoryId">The optional category identifier.</param>
    [HttpGet]
    public async Task<IActionResult> GetGamesAsync([FromQuery] int? categoryId)
    {
        var result = categoryId.HasValue
            ? await _gameService.GetGamesByCategoryAsync(categoryId.Value)
            : await _gameService.GetAllGamesAsync();

        return HandleResult(result);
    }

    /// <summary>
    /// Retrieves a game by identifier.
    /// </summary>
    /// <param name="id">The game identifier.</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGameDetailAsync(int id)
    {
        var result = await _gameService.GetGameDetailAsync(id);
        return HandleResult(result);
    }

    /// <summary>
    /// Retrieves a game by code.
    /// </summary>
    /// <param name="code">The game code.</param>
    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetGameByCodeAsync(string code)
    {
        var result = await _gameService.GetGameByCodeAsync(code);
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
