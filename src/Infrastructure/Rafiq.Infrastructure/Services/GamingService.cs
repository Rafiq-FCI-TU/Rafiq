using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;

namespace Rafiq.Infrastructure.Services;
public class GamingService(IUnitOfWork unitOfWork) : IGamingService
{
    public async Task<GamingDto> InitializeGamingAsync(int patientId)
    {
        var existingGamings = await unitOfWork.Repository<Gaming>().GetAllAsync();
        var gaming = existingGamings.FirstOrDefault(g => g.PatientId == patientId);

        if (gaming == null)
        {
            gaming = new Gaming
            {
                PatientId = patientId,
                CreatedAt = DateTime.UtcNow
            };
            await unitOfWork.Repository<Gaming>().AddAsync(gaming);
            await unitOfWork.CompleteAsync();
        }

        return await GetGamingByPatientAsync(patientId);
    }

    public async Task<GamingDto> GetGamingByPatientAsync(int patientId)
    {
        var allGamings = await unitOfWork.Repository<Gaming>().GetAllAsync();
        var gaming = allGamings.FirstOrDefault(g => g.PatientId == patientId);

        if (gaming is null)
            return new GamingDto();

        // Load all global categories
        var categories = await unitOfWork.Repository<GameCategory>().GetAllAsync();

        // Load games and patient-specific progress
        var allGames = await unitOfWork.Repository<Game>().GetAllAsync();
        var allProgress = await unitOfWork.Repository<GameProgress>().GetAllAsync();
        var patientProgress = allProgress.Where(p => p.PatientId == patientId).ToList();

        var categoryDtos = new List<GameCategoryDto>();
        foreach (var category in categories)
        {
            var games = allGames.Where(g => g.CategoryId == category.Id).ToList();
            var gameDtos = games.Select(g =>
            {
                var progressRecords = patientProgress.Where(p => p.GameId == g.Id).ToList();
                return new GameDto
                {
                    Id = g.Id,
                    CategoryId = g.CategoryId,
                    Title = g.Title,
                    MaxPoints = g.MaxPoints,
                    Duration = g.Duration,
                    Status = g.Status.ToString(),
                    Progress = g.MaxPoints > 0 && progressRecords.Any()
                        ? (double)progressRecords.Sum(p => p.AchievedPoints) / g.MaxPoints * 100
                        : 0,
                    CreatedAt = g.CreatedAt
                };
            }).ToList();

            categoryDtos.Add(new GameCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                InitialScore = category.InitialScore,
                Progress = gameDtos.Any() ? gameDtos.Average(g => g.Progress) : 0,
                Games = gameDtos
            });
        }

        return new GamingDto
        {
            Id = gaming.Id,
            PatientId = gaming.PatientId,
            TotalScore = gaming.TotalScore,
            CreatedAt = gaming.CreatedAt,
            OverallProgress = categoryDtos.Any() ? categoryDtos.Average(c => c.Progress) : 0,
            Categories = categoryDtos
        };
    }

    public async Task<IReadOnlyList<GameDto>> GetGamesByCategoryAsync(int categoryId)
    {
        var allGames = await unitOfWork.Repository<Game>().GetAllAsync();
        var games = allGames.Where(g => g.CategoryId == categoryId).ToList();

        var allProgress = await unitOfWork.Repository<GameProgress>().GetAllAsync();

        return games.Select(g =>
        {
            var progressRecords = allProgress.Where(p => p.GameId == g.Id).ToList();
            return new GameDto
            {
                Id = g.Id,
                CategoryId = g.CategoryId,
                Title = g.Title,
                MaxPoints = g.MaxPoints,
                Duration = g.Duration,
                Status = g.Status.ToString(),
                Progress = g.MaxPoints > 0 && progressRecords.Any()
                    ? (double)progressRecords.Sum(p => p.AchievedPoints) / g.MaxPoints * 100
                    : 0,
                CreatedAt = g.CreatedAt
            };
        }).ToList();
    }

    public async Task<GameProgressDto> RecordGameProgressAsync(CreateGameProgressDto dto)
    {
        var gameProgress = new GameProgress
        {
            GameId = dto.GameId,
            PatientId = dto.PatientId,
            AchievedPoints = dto.AchievedPoints,
            TimePlayed = dto.TimePlayed,
            Completed = dto.Completed,
            PlayedAt = DateTime.UtcNow
        };

        await unitOfWork.Repository<GameProgress>().AddAsync(gameProgress);

        // Update game status if completed
        if (dto.Completed)
        {
            var game = await unitOfWork.Repository<Game>().GetEntityAsync(dto.GameId);
            if (game is not null)
            {
                game.Status = GameStatus.Completed;
                unitOfWork.Repository<Game>().Update(game);
            }
        }

        await unitOfWork.CompleteAsync();

        return new GameProgressDto
        {
            Id = gameProgress.Id,
            GameId = gameProgress.GameId,
            PatientId = gameProgress.PatientId,
            AchievedPoints = gameProgress.AchievedPoints,
            TimePlayed = gameProgress.TimePlayed,
            Completed = gameProgress.Completed,
            PlayedAt = gameProgress.PlayedAt
        };
    }

    public async Task<IReadOnlyList<GameProgressDto>> GetGameProgressAsync(int gameId, int patientId)
    {
        var allProgress = await unitOfWork.Repository<GameProgress>().GetAllAsync();
        var progressRecords = allProgress
            .Where(p => p.GameId == gameId && p.PatientId == patientId)
            .OrderByDescending(p => p.PlayedAt)
            .ToList();

        return progressRecords.Select(p => new GameProgressDto
        {
            Id = p.Id,
            GameId = p.GameId,
            PatientId = p.PatientId,
            AchievedPoints = p.AchievedPoints,
            TimePlayed = p.TimePlayed,
            Completed = p.Completed,
            PlayedAt = p.PlayedAt
        }).ToList();
    }

    public async Task UpdateGameStatusAsync(int gameId, GameStatus status)
    {
        var game = await unitOfWork.Repository<Game>().GetEntityAsync(gameId);
        if (game is not null)
        {
            game.Status = status;
            unitOfWork.Repository<Game>().Update(game);
            await unitOfWork.CompleteAsync();
        }
    }
}
