using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Games;

public interface IGameService
{
    /// <summary>
    /// Retrieves all games with their category information.
    /// </summary>
    Task<Result<IReadOnlyList<GameSummaryDto>>> GetAllGamesAsync();

    /// <summary>
    /// Retrieves all games within a specific category.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    Task<Result<IReadOnlyList<GameSummaryDto>>> GetGamesByCategoryAsync(int categoryId);

    /// <summary>
    /// Retrieves a game with its level details.
    /// </summary>
    /// <param name="gameId">The game identifier.</param>
    Task<Result<GameDetailDto>> GetGameDetailAsync(int gameId);

    /// <summary>
    /// Retrieves a game with its level details by code.
    /// </summary>
    /// <param name="code">The game code.</param>
    Task<Result<GameDetailDto>> GetGameByCodeAsync(string code);
}
