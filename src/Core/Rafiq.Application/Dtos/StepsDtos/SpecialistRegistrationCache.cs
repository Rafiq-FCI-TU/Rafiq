namespace Rafiq.Application.Dtos.StepsDtos;

public class SpecialistRegistrationCache
{
    public string Email { get; set; } = null!;
    public SpecialistStep1Dto SpecialistData { get; set; } = null!;
    public int CurrentStep { get; set; } = 1;
}