using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Games;

public interface IGameLevelService
{
    /// <summary>
    /// Retrieves all levels that belong to a game.
    /// </summary>
    /// <param name="gameId">The game identifier.</param>
    Task<Result<IReadOnlyList<GameLevelSummaryDto>>> GetLevelsByGameAsync(int gameId);

    /// <summary>
    /// Retrieves a level with its questions and options.
    /// </summary>
    /// <param name="levelId">The level identifier.</param>
    Task<Result<GameLevelDetailDto>> GetLevelWithQuestionsAsync(int levelId);
}
