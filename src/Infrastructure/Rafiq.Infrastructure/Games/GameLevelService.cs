//using Rafiq.Application.Common;
using Rafiq.Application.Games;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Games;

public class GameLevelService : IGameLevelService
{
    private readonly IUnitOfWork _unitOfWork;

    public GameLevelService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Retrieves all levels that belong to a game.
    /// </summary>
    /// <param name="gameId">The game identifier.</param>
    public async Task<Result<IReadOnlyList<GameLevelSummaryDto>>> GetLevelsByGameAsync(int gameId)
    {
        try
        {
            var gameSpec = new BaseSpecifications<Game>
            {
                WhereCriteria = game => game.Id == gameId
            };
            var gameExists = await _unitOfWork.Repository<Game>().GetEntityAsync(gameSpec);

            if (gameExists is null)
            {
                return Result.Failure<IReadOnlyList<GameLevelSummaryDto>>(404, "Game not found.");
            }

            var levelSpec = new BaseSpecifications<GameLevel>
            {
                WhereCriteria = level => level.GameId == gameId,
                OrderBy = level => level.Id
            };
            levelSpec.AddInclude("Questions");

            var levels = await _unitOfWork.Repository<GameLevel>().GetAllAsync(levelSpec);
            var levelsDto = levels.Select(level => new GameLevelSummaryDto
            {

             Id = level.Id,
            Difficulty = level.Difficulty,
            LevelPoints = level.LevelPoints,
            PointsPerQuestion = level.PointsPerQuestion,
            UnlockThreshold = level.UnlockThreshold,
            QuestionCount = level.Questions.Count
        }).ToList();

            return Result.Success<IReadOnlyList<GameLevelSummaryDto>>(levelsDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<GameLevelSummaryDto>>(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a level with its questions and options.
    /// </summary>
    /// <param name="levelId">The level identifier.</param>
    public async Task<Result<GameLevelDetailDto>> GetLevelWithQuestionsAsync(int levelId)
    {
        try
        {
            var levelSpec = new BaseSpecifications<GameLevel>
            {
                WhereCriteria = level => level.Id == levelId
            };
            levelSpec.AddIncludeWithThenInclude(level => level.Questions, "Options");

            var level = await _unitOfWork.Repository<GameLevel>().GetEntityAsync(levelSpec);

            if (level is null)
            {
                return Result.Failure<GameLevelDetailDto>(404, "Game level not found.");
            }

            var result = new GameLevelDetailDto
            {
                Id = level.Id,
                Difficulty = level.Difficulty,
                LevelPoints = level.LevelPoints,
                PointsPerQuestion = level.PointsPerQuestion,
                UnlockThreshold = level.UnlockThreshold,
                QuestionCount = level.Questions.Count,
                Questions = level.Questions
                    .OrderBy(question => question.Id)
                    .Select(question => new GameQuestionDto
                    {
                        Id = question.Id,
                        Prompt = question.Prompt,
                        TargetLabel = question.TargetLabel,
                        TargetEmoji = question.TargetEmoji,
                        MetaJson = question.MetaJson,
                        Options = question.Options
                            .OrderBy(option => option.Id)
                            .Select(option => new GameOptionDto
                            {
                                Id = option.Id,
                                Label = option.Label,
                                Emoji = option.Emoji,
                                ColorHex = option.ColorHex,
                                IsCorrect = option.IsCorrect
                            })
                            .ToList()
                    })
                    .ToList()
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<GameLevelDetailDto>(500, ex.Message);
        }
    }
}