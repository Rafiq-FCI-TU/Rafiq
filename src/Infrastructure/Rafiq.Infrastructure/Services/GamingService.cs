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
            await unitOfWork.CompleteAsync(); // Saves and gets the assigned gaming ID
        }

        var existingCategories = await unitOfWork.Repository<GameCategory>().GetAllAsync();
        if (!existingCategories.Any(c => c.GamingId == gaming.Id))
        {
            var categories = new List<GameCategory>
            {
                new GameCategory { GamingId = gaming.Id, Name = "Cognitive Skills", InitialScore = 0 },
                new GameCategory { GamingId = gaming.Id, Name = "Memory Training", InitialScore = 0 }
            };

            foreach (var category in categories)
            {
                await unitOfWork.Repository<GameCategory>().AddAsync(category);
            }
            await unitOfWork.CompleteAsync();

            var games = new List<Game>
            {
                // Puzzles under Cognitive
                new Game { CategoryId = categories[0].Id, Title = "Match Shapes", MaxPoints = 100, Duration = "5 mins", Status = GameStatus.NotStarted, CreatedAt = DateTime.UtcNow },
                new Game { CategoryId = categories[0].Id, Title = "Logic Puzzle", MaxPoints = 200, Duration = "10 mins", Status = GameStatus.NotStarted, CreatedAt = DateTime.UtcNow },
                // Memory under Memory Training
                new Game { CategoryId = categories[1].Id, Title = "Card Flip", MaxPoints = 150, Duration = "8 mins", Status = GameStatus.NotStarted, CreatedAt = DateTime.UtcNow }
            };

            foreach (var game in games)
            {
                await unitOfWork.Repository<Game>().AddAsync(game);
            }
            await unitOfWork.CompleteAsync();
        }

        // Return the fully populated gaming profile with all newly created categories and games
        return await GetGamingByPatientAsync(patientId);
    }

    public async Task<GamingDto> GetGamingByPatientAsync(int patientId)
    {
        var allGamings = await unitOfWork.Repository<Gaming>().GetAllAsync();
        var gaming = allGamings.FirstOrDefault(g => g.PatientId == patientId);

        if (gaming is null)
            return new GamingDto();

        // Load categories
        var allCategories = await unitOfWork.Repository<GameCategory>().GetAllAsync();
        var categories = allCategories.Where(c => c.GamingId == gaming.Id).ToList();

        // Load games for each category
        var allGames = await unitOfWork.Repository<Game>().GetAllAsync();
        var allProgress = await unitOfWork.Repository<GameProgress>().GetAllAsync();

        var categoryDtos = new List<GameCategoryDto>();
        foreach (var category in categories)
        {
            var games = allGames.Where(g => g.CategoryId == category.Id).ToList();
            var gameDtos = games.Select(g =>
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

            categoryDtos.Add(new GameCategoryDto
            {
                Id = category.Id,
                GamingId = category.GamingId,
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

    private static GamingDto MapToGamingDto(Gaming gaming)
    {
        return new GamingDto
        {
            Id = gaming.Id,
            PatientId = gaming.PatientId,
            TotalScore = gaming.TotalScore,
            CreatedAt = gaming.CreatedAt,
            OverallProgress = 0,
            Categories = []
        };
    }
}
