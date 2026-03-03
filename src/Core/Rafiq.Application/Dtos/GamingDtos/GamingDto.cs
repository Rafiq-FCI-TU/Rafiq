namespace Rafiq.Application.Dtos.GamingDtos;
public class GamingDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int TotalScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public double OverallProgress { get; set; }
    public List<GameCategoryDto> Categories { get; set; } = [];
}
