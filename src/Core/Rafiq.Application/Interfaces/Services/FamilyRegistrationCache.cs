using Rafiq.Application.Dtos.StepsDtos;

namespace Rafiq.Application.Interfaces.Services;
public class FamilyRegistrationCache
{
    public string Email { get; set; } = null!;
    public FamilyStep1Dto FamilyData { get; set; } = null!;
    public PatientStep2Dto? PatientData { get; set; }
    public int CurrentStep { get; set; } = 1;
}
