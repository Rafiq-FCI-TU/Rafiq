namespace Rafiq.Application.Dtos.GamingDtos;
public class CreateGameProgressDto
{
    public int GameId { get; set; }
    public int PatientId { get; set; }
    public int AchievedPoints { get; set; }
    public int TimePlayed { get; set; }
    public bool Completed { get; set; }
}
