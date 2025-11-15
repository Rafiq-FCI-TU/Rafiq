using Rafiq.Application.Dtos;

namespace Rafiq.Application.Interfaces.Services;
public class FamilyRegistrationCache
{
    public string Email { get; set; }
    public FamilyStep1Dto FamilyData { get; set; }
    public PatientStep2Dto? PatientData { get; set; }
    public int CurrentStep { get; set; } = 1;
}
