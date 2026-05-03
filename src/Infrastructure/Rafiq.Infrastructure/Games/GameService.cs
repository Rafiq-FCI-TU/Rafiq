using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Specifications;
using Rafiq.Application.Games;
using Rafiq.Domain.ErrorHandling;
using Rafiq.Domain.Entities;

namespace Rafiq.Infrastructure.Games;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;

    public GameService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Retrieves all games with their category information.
    /// </summary>
    public async Task<Result<IReadOnlyList<GameSummaryDto>>> GetAllGamesAsync()
    {
        try
        {
            var gamesEntities = await _unitOfWork.Repository<Game>().GetAllAsync();
            var games = gamesEntities.Select(game => new GameSummaryDto
            {
                Id = game.Id,
                Code = game.Code,
                Title = game.Title,
                Description = game.Description,
                QuestionType = game.QuestionType,
                CategoryId = game.CategoryId,
                CategoryName = game.Category?.Name ?? ""
            }).ToList();

            return Result.Success<IReadOnlyList<GameSummaryDto>>(games);
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<GameSummaryDto>>(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves all games within a specific category.
    /// </summary>
    /// <param name="categoryId">The category identifier.</param>
    public async Task<Result<IReadOnlyList<GameSummaryDto>>> GetGamesByCategoryAsync(int categoryId)
    {
        try
        {
            var categoryRepo = _unitOfWork.Repository<Category>();
            var categoryExists = (await categoryRepo.GetAllAsync(new BaseSpecifications<Category> { WhereCriteria = c => c.Id == categoryId })).Any();

            if (!categoryExists)
            {
                return Result.Failure<IReadOnlyList<GameSummaryDto>>(404, "Category not found.");
            }

            var gamesEntities = await _unitOfWork.Repository<Game>()
                .GetAllAsync(new BaseSpecifications<Game> { WhereCriteria = game => game.CategoryId == categoryId });
            var games = gamesEntities.Select(game => new GameSummaryDto
            {
                Id = game.Id,
                Code = game.Code,
                Title = game.Title,
                Description = game.Description,
                QuestionType = game.QuestionType,
                CategoryId = game.CategoryId,
                CategoryName = game.Category?.Name ?? ""
            }).ToList();

            return Result.Success<IReadOnlyList<GameSummaryDto>>(games);
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<GameSummaryDto>>(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a game with its level details.
    /// </summary>
    /// <param name="gameId">The game identifier.</param>
    public async Task<Result<GameDetailDto>> GetGameDetailAsync(int gameId)
    {
        try
        {
            var spec = new BaseSpecifications<Game> { WhereCriteria = game => game.Id == gameId };
            spec.AddInclude("Category");
            spec.AddIncludeWithThenInclude(g => g.Levels, "Questions");

            var gameEntity = await _unitOfWork.Repository<Game>().GetEntityAsync(spec);

            if (gameEntity is null)
            {
                return Result.Failure<GameDetailDto>(404, "Game not found.");
            }

            var game = MapToGameDetailDto(gameEntity);
            return Result.Success(game);
        }
        catch (Exception ex)
        {
            return Result.Failure<GameDetailDto>(500, ex.Message);
        }
    }

    /// <summary>
    /// Retrieves a game with its level details by code.
    /// </summary>
    /// <param name="code">The game code.</param>
    public async Task<Result<GameDetailDto>> GetGameByCodeAsync(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return Result.Failure<GameDetailDto>(400, "Game code is required.");
            }

            var normalizedCode = code.Trim();

            var spec = new BaseSpecifications<Game> { WhereCriteria = game => game.Code == normalizedCode };
            spec.AddInclude("Category");
            spec.AddIncludeWithThenInclude(g => g.Levels, "Questions");

            var gameEntity = await _unitOfWork.Repository<Game>().GetEntityAsync(spec);

            if (gameEntity is null)
            {
                return Result.Failure<GameDetailDto>(404, "Game not found.");
            }

            var game = MapToGameDetailDto(gameEntity);
            return Result.Success(game);
        }
        catch (Exception ex)
        {
            return Result.Failure<GameDetailDto>(500, ex.Message);
        }
    }

    private GameDetailDto MapToGameDetailDto(Game game)
    {
        return new GameDetailDto
        {
            Id = game.Id,
            Code = game.Code,
            Title = game.Title,
            Description = game.Description,
            QuestionType = game.QuestionType,
            FeedbackCorrect = game.FeedbackCorrect,
            FeedbackWrong = game.FeedbackWrong,
            CategoryId = game.CategoryId,
            CategoryName = game.Category?.Name ?? "",
            Levels = game.Levels
                .OrderBy(level => level.Id)
                .Select(level => new GameLevelSummaryDto
                {
                    Id = level.Id,
                    Difficulty = level.Difficulty ?? "",
                    LevelPoints = level.LevelPoints,
                    PointsPerQuestion = level.PointsPerQuestion,
                    UnlockThreshold = level.UnlockThreshold,
                    QuestionCount = level.Questions?.Count ?? 0
                })
                .ToList()
        };
    }
}
