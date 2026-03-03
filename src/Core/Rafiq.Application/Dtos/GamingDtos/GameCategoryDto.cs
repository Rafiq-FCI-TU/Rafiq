namespace Rafiq.Application.Dtos.GamingDtos;
public class GameCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int InitialScore { get; set; }
    public double Progress { get; set; }
    public List<GameDto> Games { get; set; } = [];
}
