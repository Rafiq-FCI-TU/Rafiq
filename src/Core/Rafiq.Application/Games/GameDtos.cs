namespace Rafiq.Application.Games;

public class GameSummaryDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

public class GameDetailDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public string FeedbackCorrect { get; set; } = string.Empty;
    public string FeedbackWrong { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public IReadOnlyList<GameLevelSummaryDto> Levels { get; set; } = [];
}
