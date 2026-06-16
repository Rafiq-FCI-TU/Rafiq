namespace Rafiq.Application.Dtos;
public class ResponseDto
{
    public bool IsAuthenticated { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Roles { get; set; } = [];
    public string Token { get; set; } = null!;
    public DateTime ExpiresOn { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }


}
public class ResponseForFamilyRegistrationDto : ResponseDto
{
    public string Id { get; set; }

    public string ?Specialistid { get; set; }

    public double? score{ get; set; } 
    public int? Patientid { get; set; }

    public bool? HasAssessment { get; set; }
    public int? AssessmentId { get; set; }


}