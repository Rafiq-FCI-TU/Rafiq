using AutoMapper;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services;

public class GamingService : IGamingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GamingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    #region Get Player Overview

    public async Task<Result<PlayerGamingOverviewDto>> GetPlayerOverviewAsync(int patientId)
    {
        try
        {
            // 1. Load patient to get TotalScore
            var patient = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientId);
            if (patient == null)
                return Result.Failure<PlayerGamingOverviewDto>(404, "Patient not found.");

            // 2. Load all global categories with Games → Levels in a single query
            var categorySpec = new BaseSpecifications<GameCategory>();
            categorySpec.AddIncludeWithThenInclude(c => c.Games, "Levels");

            var categories = await _unitOfWork.Repository<GameCategory>().GetAllAsync(categorySpec);
            if (!categories.Any())
                return Result.Failure<PlayerGamingOverviewDto>(404, "No game categories found.");

            // 3. Map to DTOs and compute IsUnlocked for each level
            var overview = new PlayerGamingOverviewDto
            {
                PatientId = patientId,
                TotalScore = patient.TotalScore,
                Categories = new List<GameCategoryDto>()
            };

            foreach (var category in categories)
            {
                var categoryDto = _mapper.Map<GameCategoryDto>(category);

                foreach (var gameDto in categoryDto.Games)
                {
                    var game = category.Games.First(g => g.Id == gameDto.Id);

                    for (int i = 0; i < gameDto.Levels.Count; i++)
                    {
                        var level = game.Levels.First(l => l.Id == gameDto.Levels[i].Id);
                        gameDto.Levels[i].IsUnlocked = patient.TotalScore >= level.RequiredScore;
                    }
                }

                overview.Categories.Add(categoryDto);
            }

            return Result.Success(overview);
        }
        catch (Exception ex)
        {
            return Result.Failure<PlayerGamingOverviewDto>(500, $"Failed to get player overview: {ex.Message}");
        }
    }

    #endregion

    #region Submit Game Result

    public async Task<Result<GameResultResponseDto>> SubmitGameResultAsync(int patientId, SubmitGameResultDto dto)
    {
        try
        {
            // 1. Load patient
            var patient = await _unitOfWork.Repository<Patient>().GetEntityAsync(patientId);
            if (patient == null)
                return Result.Failure<GameResultResponseDto>(404, "Patient not found.");

            // 2. Load the GameLevel
            var gameLevelSpec = new BaseSpecifications<GameLevel>
            {
                WhereCriteria = gl => gl.Id == dto.GameLevelId
            };
            gameLevelSpec.AddInclude("Game");

            var gameLevel = await _unitOfWork.Repository<GameLevel>().GetEntityAsync(gameLevelSpec);
            if (gameLevel == null)
                return Result.Failure<GameResultResponseDto>(404, "Game level not found.");

            // 3. Check if level is unlocked
            if (patient.TotalScore < gameLevel.RequiredScore)
                return Result.Failure<GameResultResponseDto>(400,
                    $"Level is locked. Required score: {gameLevel.RequiredScore}, current score: {patient.TotalScore}.");

            // 4. Record progress
            var progress = new GameProgress
            {
                PatientId = patientId,
                GameLevelId = dto.GameLevelId,
                EarnedPoints = gameLevel.RewardPoints,
                PlayedAt = DateTime.Now
            };

            await _unitOfWork.Repository<GameProgress>().AddAsync(progress);

            // 5. Update patient's total score
            patient.TotalScore += gameLevel.RewardPoints;
            _unitOfWork.Repository<Patient>().Update(patient);

            await _unitOfWork.CompleteAsync();

            // 6. Find newly unlocked levels
            var allLevelsSpec = new BaseSpecifications<GameLevel>
            {
                WhereCriteria = gl => gl.RequiredScore > (patient.TotalScore - gameLevel.RewardPoints)
                                   && gl.RequiredScore <= patient.TotalScore
            };
            allLevelsSpec.AddInclude("Game");

            var newlyUnlocked = await _unitOfWork.Repository<GameLevel>().GetAllAsync(allLevelsSpec);

            var response = new GameResultResponseDto
            {
                EarnedPoints = gameLevel.RewardPoints,
                NewTotalScore = patient.TotalScore,
                NewlyUnlockedLevels = newlyUnlocked
                    .Select(l => $"{l.Game.Title} - {l.DifficultyLevel}")
                    .ToList()
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            return Result.Failure<GameResultResponseDto>(500, $"Failed to submit game result: {ex.Message}");
        }
    }

    #endregion
}
