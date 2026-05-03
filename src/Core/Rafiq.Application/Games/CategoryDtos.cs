namespace Rafiq.Application.Games;

public class CategoryOverviewDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int GameCount { get; set; }
}

public class CategoryWithGamesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IReadOnlyList<GameSummaryDto> Games { get; set; } = [];
}
