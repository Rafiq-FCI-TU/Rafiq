namespace Rafiq.Application.Games;

public class SubmitPatientLevelRequest
{
    public int PatientId { get; set; }
    public int GameLevelId { get; set; }
    public double EarnedPoints { get; set; }
}

public class PatientLevelResultDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int GameLevelId { get; set; }
    public double EarnedPoints { get; set; }
    public double MaxPoints { get; set; }
    public double Score { get; set; }
    public int Stars { get; set; }
    public DateTime PlayedAt { get; set; }
}

public class PatientProgressSummaryDto
{
    public int PatientId { get; set; }
    public int TotalGamesPlayed { get; set; }
    public double TotalEarnedPoints { get; set; }
    public IReadOnlyList<CategoryProgressDto> ByCategory { get; set; } = [];
}

public class CategoryProgressDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int GamesPlayed { get; set; }
    public double TotalPoints { get; set; }
}
