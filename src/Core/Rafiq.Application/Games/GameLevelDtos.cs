namespace Rafiq.Application.Games;

public class GameLevelSummaryDto
{
    public int Id { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public int LevelPoints { get; set; }
    public int PointsPerQuestion { get; set; }
    public int UnlockThreshold { get; set; }
    public int QuestionCount { get; set; }
}

public class GameLevelDetailDto
{
    public int Id { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public int LevelPoints { get; set; }
    public int PointsPerQuestion { get; set; }
    public int UnlockThreshold { get; set; }
    public int QuestionCount { get; set; }
    public IReadOnlyList<GameQuestionDto> Questions { get; set; } = [];
}
