using Rafiq.Domain.Enums;

namespace Rafiq.Application.Dtos.GamingDtos;

public class GamingOverviewLevelDto
{
    public int Id { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public int RequiredScore { get; set; }
    public int MaxScore { get; set; }
    public bool IsUnlocked { get; set; }
    public int BestScore { get; set; }
}

public class GamingOverviewGameDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<GamingOverviewLevelDto> Levels { get; set; } = [];
}

public class GamingOverviewCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<GamingOverviewGameDto> Games { get; set; } = [];
}

public class SubmitGamePlayRequestDto
{
    public int PatientId { get; set; }
    public int GameLevelId { get; set; }
    public int Score { get; set; }
}

public class SubmitGamePlayResponseDto
{
    public int LevelId { get; set; }
    public int Score { get; set; }
    public bool Passed { get; set; }
    public int RequiredScore { get; set; }
    public int MaxScore { get; set; }
    public int NewBestScore { get; set; }
    public bool NextLevelUnlocked { get; set; }
}
