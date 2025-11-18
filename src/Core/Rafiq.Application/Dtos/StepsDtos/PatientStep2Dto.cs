namespace Rafiq.Application.Dtos.StepsDtos;
public class PatientStep2Dto
{
    public string Token { get; init; } = null!;
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public DateTime DateOfBirth { get; init; }
    public string Gender { get; init; } = null!;
}