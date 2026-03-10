using Rafiq.Domain.Enums;

namespace Rafiq.Application.Dtos.GamingDtos;

public class GameLevelDto
{
    public int Id { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public int RequiredScore { get; set; }
    public int RewardPoints { get; set; }
    public bool IsUnlocked { get; set; }
}

public class GameDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public List<GameLevelDto> Levels { get; set; } = new();
}

public class GameCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<GameDto> Games { get; set; } = new();
}

public class PlayerGamingOverviewDto
{
    public int PatientId { get; set; }
    public double TotalScore { get; set; }
    public List<GameCategoryDto> Categories { get; set; } = new();
}

public class SubmitGameResultDto
{
    public int GameLevelId { get; set; }
}

public class GameResultResponseDto
{
    public int EarnedPoints { get; set; }
    public double NewTotalScore { get; set; }
    public List<string> NewlyUnlockedLevels { get; set; } = new();
}
