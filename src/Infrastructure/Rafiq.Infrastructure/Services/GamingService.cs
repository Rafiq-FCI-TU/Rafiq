using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services;

public class GamingService : IGamingService
{
    private readonly IUnitOfWork _unitOfWork;

    public GamingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IReadOnlyList<GamingOverviewCategoryDto>>> GetGamingOverviewAsync(int patientId)
    {
        try
        {
            var patient = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientId);
            if (patient == null)
            {
                return Result.Failure<IReadOnlyList<GamingOverviewCategoryDto>>(404, "Patient not found.");
            }

            var categoriesSpec = new BaseSpecifications<GameCategory>();
            categoriesSpec.AddIncludeWithThenInclude(category => category.Games, "Levels");

            var categories = await _unitOfWork.Repository<GameCategory>().GetAllAsync(categoriesSpec);
            var levels = categories
                .SelectMany(category => category.Games)
                .SelectMany(game => game.Levels)
                .ToList();
            var levelIds = levels.Select(level => level.Id).ToList();

            if (levels.Count == 0)
            {
                return Result.Success<IReadOnlyList<GamingOverviewCategoryDto>>(Array.Empty<GamingOverviewCategoryDto>());
            }

            var patientLevelSpec = new BaseSpecifications<PatientLevel>
            {
                WhereCriteria = patientLevel =>
                    patientLevel.PatientId == patientId &&
                    levelIds.Contains(patientLevel.GameLevelId)
            };

            var patientLevels = await _unitOfWork.Repository<PatientLevel>().GetAllAsync(patientLevelSpec);
            var bestScoresByLevelId = patientLevels
                .GroupBy(patientLevel => patientLevel.GameLevelId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Max(patientLevel => patientLevel.EarnedPoints));

            var overview = categories
                .Select(category => new GamingOverviewCategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Games = category.Games
                        .OrderBy(game => game.Id)
                        .Select(game => new GamingOverviewGameDto
                        {
                            Id = game.Id,
                            Title = game.Title,
                            Description = game.Description,
                            Levels = game.Levels
                                .OrderBy(level => level.DifficultyLevel)
                                .Select(level => new GamingOverviewLevelDto
                                {
                                    Id = level.Id,
                                    DifficultyLevel = level.DifficultyLevel,
                                    RequiredScore = level.RequiredScore,
                                    MaxScore = level.MaxScore,
                                    BestScore = bestScoresByLevelId.GetValueOrDefault(level.Id),
                                    IsUnlocked = IsLevelUnlocked(level, game.Levels, bestScoresByLevelId)
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return Result.Success<IReadOnlyList<GamingOverviewCategoryDto>>(overview);
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<GamingOverviewCategoryDto>>(500, $"Failed to get gaming overview: {ex.Message}");
        }
    }

    public async Task<Result<SubmitGamePlayResponseDto>> SubmitGameResultAsync(SubmitGamePlayRequestDto dto)
    {
        try
        {
            var patient = await _unitOfWork.Repository<Patient>().GetEntityAsync(dto.PatientId);
            if (patient == null)
            {
                return Result.Failure<SubmitGamePlayResponseDto>(404, "Patient not found.");
            }

            var gameLevelSpec = new BaseSpecifications<GameLevel>
            {
                WhereCriteria = level => level.Id == dto.GameLevelId
            };
            gameLevelSpec.AddIncludeWithThenInclude(level => level.Game, "Levels");

            var gameLevel = await _unitOfWork.Repository<GameLevel>().GetEntityAsync(gameLevelSpec);
            if (gameLevel == null)
            {
                return Result.Failure<SubmitGamePlayResponseDto>(404, "Game level not found.");
            }

            var gameLevels = gameLevel.Game.Levels.OrderBy(level => level.DifficultyLevel).ToList();
            var sameGameLevelIds = gameLevels.Select(level => level.Id).ToList();

            var patientLevelSpec = new BaseSpecifications<PatientLevel>
            {
                WhereCriteria = patientLevel =>
                    patientLevel.PatientId == dto.PatientId &&
                    sameGameLevelIds.Contains(patientLevel.GameLevelId)
            };

            var patientLevels = await _unitOfWork.Repository<PatientLevel>().GetAllAsync(patientLevelSpec);
            var bestScoresByLevelId = patientLevels
                .GroupBy(patientLevel => patientLevel.GameLevelId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Max(patientLevel => patientLevel.EarnedPoints));

            if (!IsLevelUnlocked(gameLevel, gameLevels, bestScoresByLevelId))
            {
                return Result.Failure<SubmitGamePlayResponseDto>(400, "Level is locked for this patient.");
            }

            var normalizedScore = Math.Min(dto.Score, gameLevel.MaxScore);
            var currentRecord = patientLevels
                .FirstOrDefault(patientLevel => patientLevel.GameLevelId == dto.GameLevelId);

            if (currentRecord == null)
            {
                currentRecord = new PatientLevel
                {
                    PatientId = dto.PatientId,
                    GameLevelId = dto.GameLevelId,
                    EarnedPoints = normalizedScore,
                    PlayedAt = DateTime.UtcNow
                };

                await _unitOfWork.Repository<PatientLevel>().AddAsync(currentRecord);
            }
            else if (normalizedScore > currentRecord.EarnedPoints)
            {
                currentRecord.EarnedPoints = normalizedScore;
                currentRecord.PlayedAt = DateTime.UtcNow;
                _unitOfWork.Repository<PatientLevel>().Update(currentRecord);
            }

            await _unitOfWork.CompleteAsync();

            bestScoresByLevelId[dto.GameLevelId] = Math.Max(
                bestScoresByLevelId.GetValueOrDefault(dto.GameLevelId),
                normalizedScore);

            var response = new SubmitGamePlayResponseDto
            {
                LevelId = gameLevel.Id,
                Score = normalizedScore,
                Passed = normalizedScore >= gameLevel.RequiredScore,
                RequiredScore = gameLevel.RequiredScore,
                MaxScore = gameLevel.MaxScore,
                NewBestScore = bestScoresByLevelId[dto.GameLevelId],
                NextLevelUnlocked = IsNextLevelUnlocked(gameLevel, gameLevels, bestScoresByLevelId)
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<SubmitGamePlayResponseDto>(500, $"Failed to submit game result: {ex.Message}");
        }
    }

    private static bool IsLevelUnlocked(
        GameLevel level,
        IEnumerable<GameLevel> gameLevels,
        IReadOnlyDictionary<int, int> bestScoresByLevelId)
    {
        if (level.DifficultyLevel == DifficultyLevel.Easy)
        {
            return true;
        }

        var previousLevel = GetPreviousLevel(level, gameLevels);
        if (previousLevel == null)
        {
            return false;
        }

        return bestScoresByLevelId.TryGetValue(previousLevel.Id, out var bestScore) &&
               bestScore >= previousLevel.RequiredScore;
    }

    private static bool IsNextLevelUnlocked(
        GameLevel currentLevel,
        IReadOnlyList<GameLevel> gameLevels,
        IReadOnlyDictionary<int, int> bestScoresByLevelId)
    {
        var nextLevel = GetNextLevel(currentLevel, gameLevels);
        return nextLevel != null && IsLevelUnlocked(nextLevel, gameLevels, bestScoresByLevelId);
    }

    private static GameLevel? GetPreviousLevel(GameLevel level, IEnumerable<GameLevel> gameLevels)
    {
        var previousDifficulty = level.DifficultyLevel switch
        {
            DifficultyLevel.Medium => DifficultyLevel.Easy,
            DifficultyLevel.Hard => DifficultyLevel.Medium,
            _ => (DifficultyLevel?)null
        };

        return previousDifficulty == null
            ? null
            : gameLevels.FirstOrDefault(gameLevel => gameLevel.DifficultyLevel == previousDifficulty);
    }

    private static GameLevel? GetNextLevel(GameLevel level, IEnumerable<GameLevel> gameLevels)
    {
        var nextDifficulty = level.DifficultyLevel switch
        {
            DifficultyLevel.Easy => DifficultyLevel.Medium,
            DifficultyLevel.Medium => DifficultyLevel.Hard,
            _ => (DifficultyLevel?)null
        };

        return nextDifficulty == null
            ? null
            : gameLevels.FirstOrDefault(gameLevel => gameLevel.DifficultyLevel == nextDifficulty);
    }
}
