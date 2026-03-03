namespace Rafiq.Application.Dtos.GamingDtos;
public class GameDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int MaxPoints { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public double Progress { get; set; }
    public DateTime CreatedAt { get; set; }
}
