using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Common;
using Rafiq.Application.Games;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Games;

public class PatientLevelService : IPatientLevelService
{
    private readonly IUnitOfWork _unitOfWork;

    public PatientLevelService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Persists a patient level result and returns the computed outcome.
    /// </summary>
    /// <param name="request">The patient level submission payload.</param>
    public async Task<Result<PatientLevelResultDto>> SubmitLevelResultAsync(SubmitPatientLevelRequest request)
    {
        try
        {
            if (request is null)
            {
                return Result.Failure<PatientLevelResultDto>(400, "Request is required.");
            }

            if (request.EarnedPoints < 0)
            {
                return Result.Failure<PatientLevelResultDto>(400, "EarnedPoints cannot be negative.");
            }

            var patientspec = new BaseSpecifications<Patient>{
                WhereCriteria = patient => patient.Id == request.PatientId
            };

            var patientExists = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientspec);

            if (patientExists is null)
            {
                return Result.Failure<PatientLevelResultDto>(404, "Patient not found.");
            }

            var Gamespec = new BaseSpecifications<GameLevel>
            {
                WhereCriteria = GameLevel => GameLevel.Id == request.GameLevelId
            };

            var gameLevel = await _unitOfWork.Repository<GameLevel>().GetEntityAsync(Gamespec);

            if (gameLevel is null)
            {
                return Result.Failure<PatientLevelResultDto>(404, "Game level not found.");
            }

            // Real logic

            var maxPoints = gameLevel.LevelPoints;

            // current game level score
            var score = (request.EarnedPoints / maxPoints) * 100;

            var patientLevel = new PatientLevel
            {
                PatientId = request.PatientId,
                GameLevelId = request.GameLevelId,
                EarnedPoints = request.EarnedPoints,
                MaxPoints = maxPoints,
                Score = score,
                PlayedAt = DateTime.Now
            };

            await _unitOfWork.Repository<PatientLevel>().AddAsync(patientLevel);

            await _unitOfWork.CompleteAsync();

            // Now we need to recalculate category score and patient total score

            var categoies = await _unitOfWork.Repository<Category>().GetAllAsync();

            var patientScore = 0.0;

            foreach (var category in categoies)
            {
                var categoryEarnedPoints = 0.0;
                var categoryMaxPoints = 0.0;

                // For calculating category score we need to consider both practice categories (Assessment) and game levels

                // First we get practice categories (Assessment)
                var practiceCategory = await _unitOfWork.Repository<PracticeCategory>().GetEntityAsync(new BaseSpecifications<PracticeCategory>
                {
                    WhereCriteria = pc => pc.CategoryId == category.Id
                });

                if (practiceCategory is not null) {
                    categoryEarnedPoints += practiceCategory.QuestionsEarnedPoints;
                    categoryMaxPoints += practiceCategory.QuestionsMaxPoints;
                }

                // Then we get game levels score
                var gameSpec = new BaseSpecifications<Game>
                {
                    WhereCriteria = g => g.CategoryId == category.Id
                };

                gameSpec.AddIncludeWithThenInclude(a => a.Levels, "PatientLevels");

                var games = await _unitOfWork.Repository<Game>().GetAllAsync(gameSpec);

                foreach (var game in games)
                {
                    var levels = game.Levels;

                    foreach (var level in levels)
                    {
                        foreach (var patientGameLevel in level.PatientLevels)
                        {
                            categoryEarnedPoints += patientGameLevel.EarnedPoints;
                            categoryMaxPoints += patientGameLevel.MaxPoints;
                        }
                    }
                }

                var categoryScore = (categoryMaxPoints > 0) ? (categoryEarnedPoints / categoryMaxPoints) * 100 : 0;

                if (practiceCategory is not null)
                {
                    practiceCategory.Score = categoryScore;
                    _unitOfWork.Repository<PracticeCategory>().Update(practiceCategory);
                }

                patientScore += categoryScore;
            }

            patientScore /= 4;

            var patient = await _unitOfWork.Repository<Patient>().GetEntityAsync(new BaseSpecifications<Patient>
            {
                WhereCriteria = patient => patient.Id == request.PatientId
            });

            if (patient is null)
            {
                return Result.Failure<PatientLevelResultDto>(404, "Patient not found.");
            }

             patient.TotalScore = patientScore;
            _unitOfWork.Repository<Patient>().Update(patient);

            await _unitOfWork.CompleteAsync();

            var result = new PatientLevelResultDto
            {
                Id = patientLevel.Id,
                PatientId = patientLevel.PatientId,
                GameLevelId = patientLevel.GameLevelId,
                EarnedPoints = patientLevel.EarnedPoints,
                MaxPoints = patientLevel.MaxPoints,
                Score = patientLevel.Score,
                PlayedAt = patientLevel.PlayedAt
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<PatientLevelResultDto>(500, ex.Message);
        }





    }

    public async Task<Result<IReadOnlyList<PatientLevelResultDto>>> GetPatientLevelHistoryAsync(int patientId, int gameLevelId)
    {
        try
        {
            var patientSpec = new BaseSpecifications<Patient>
            {
                WhereCriteria = patient => patient.Id == patientId
            };

            var patientExists = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientSpec);

            if (patientExists is null)
                return Result.Failure<IReadOnlyList<PatientLevelResultDto>>(404, "Patient not found.");

            var levelSpec = new BaseSpecifications<GameLevel>
            {
                WhereCriteria = level => level.Id == gameLevelId
            };

            var levelExists = await _unitOfWork.Repository<GameLevel>().GetEntityAsync(levelSpec);

            if (levelExists is null)
                return Result.Failure<IReadOnlyList<PatientLevelResultDto>>(404, "Game level not found.");

            var historySpec = new BaseSpecifications<PatientLevel>
            {
                WhereCriteria = pl => pl.PatientId == patientId && pl.GameLevelId == gameLevelId
            };

            var history = await _unitOfWork.Repository<PatientLevel>().GetAllAsync(historySpec);

            var result = history
                .OrderByDescending(pl => pl.PlayedAt)
                .Select(pl => new PatientLevelResultDto
                {
                    Id = pl.Id,
                    PatientId = pl.PatientId,
                    GameLevelId = pl.GameLevelId,
                    EarnedPoints = pl.EarnedPoints,
                    MaxPoints = pl.MaxPoints,
                    Score = pl.Score,
                    Stars = CalculateStars(pl.MaxPoints <= 0 ? 0 : pl.EarnedPoints / pl.MaxPoints),
                    PlayedAt = pl.PlayedAt
                })
                .ToList();

            return Result.Success<IReadOnlyList<PatientLevelResultDto>>(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<IReadOnlyList<PatientLevelResultDto>>(500, ex.Message);
        }
    }

    public async Task<Result<PatientProgressSummaryDto>> GetPatientProgressAsync(int patientId)
    {
        try
        {
            var patientSpec = new BaseSpecifications<Patient>
            {
                WhereCriteria = patient => patient.Id == patientId
            };

            var patientExists = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientSpec);

            if (patientExists is null)
                return Result.Failure<PatientProgressSummaryDto>(404, "Patient not found.");

            // جيب كل الـ PatientLevels مع الـ GameLevel → Game
            var progressSpec = new BaseSpecifications<PatientLevel>
            {
                WhereCriteria = pl => pl.PatientId == patientId
            };

            progressSpec.AddIncludeWithThenInclude(pl => pl.GameLevel, "Game");

            var patientLevels = await _unitOfWork.Repository<PatientLevel>().GetAllAsync(progressSpec);

            // Group by Category
            var byCategory = patientLevels
                .GroupBy(pl => new
                {
                    CategoryId = pl.GameLevel.Game.CategoryId,
                    CategoryName = pl.GameLevel.Game.Category?.Name ?? "Unknown"
                })
                .OrderBy(g => g.Key.CategoryName)
                .Select(g => new CategoryProgressDto
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    GamesPlayed = g.Select(pl => pl.GameLevel.GameId).Distinct().Count(),
                    TotalPoints = g.Sum(pl => pl.EarnedPoints)
                })
                .ToList();

            var result = new PatientProgressSummaryDto
            {
                PatientId = patientId,
                TotalGamesPlayed = patientLevels.Select(pl => pl.GameLevel.GameId).Distinct().Count(),
                TotalEarnedPoints = patientLevels.Sum(pl => pl.EarnedPoints),
                ByCategory = byCategory
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<PatientProgressSummaryDto>(500, ex.Message);
        }
    }

    private static int CalculateStars(double accuracy)
    {
        if (accuracy >= 0.9d)
        {
            return 3;
        }

        if (accuracy >= 0.6d)
        {
            return 2;
        }

        if (accuracy >= 0.3d)
        {
            return 1;
        }

        return 0;
    }

    private async Task<double> CalculateScore(int patientId)
    {
        var patientLevelSpec = new BaseSpecifications<PatientLevel>
        {
            WhereCriteria = pl => pl.PatientId == patientId
        };
        patientLevelSpec.AddIncludeWithThenInclude(pl => pl.GameLevel, "Game");

        var patientLevels = await _unitOfWork.Repository<PatientLevel>().GetAllAsync(patientLevelSpec);

        if (patientLevels is null || !patientLevels.Any())
            return 0;

        var totalScore = patientLevels.Sum(pl => pl.Score);

        return Math.Round(totalScore / 4, 2);
    }

}
