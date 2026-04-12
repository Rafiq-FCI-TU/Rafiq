namespace Rafiq.Application.Dtos.PatientDtos;

public class PatientResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}".Trim();
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public double TotalScore { get; set; }
    public string FamilyProfileId { get; set; } = string.Empty;
    public int AssessmentId { get; set; }
}
