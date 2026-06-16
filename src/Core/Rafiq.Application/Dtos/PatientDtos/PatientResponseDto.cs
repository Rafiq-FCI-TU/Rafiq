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

public class PatientDoctorDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Credentials { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string ProfessionalBio { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }
}
