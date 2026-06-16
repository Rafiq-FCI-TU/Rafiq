namespace Rafiq.Application.Games;

public class GameQuestionDto
{
    public int Id { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string? TargetLabel { get; set; }
    public string? TargetEmoji { get; set; }
    public string? MetaJson { get; set; }
    public IReadOnlyList<GameOptionDto> Options { get; set; } = [];
}

public class GameOptionDto
{
    public int Id { get; set; }
    public string Label { get; set; } = string.Empty;
    public string? Emoji { get; set; }
    public string? ColorHex { get; set; }
    public bool IsCorrect { get; set; }
}
