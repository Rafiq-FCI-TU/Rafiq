using Rafiq.Application.Dtos;

namespace Rafiq.Application.Interfaces.Services;
public interface IFamilyRegistrationService
{
    Task<(string Token, int NextStep)> SaveStep1Async(FamilyStep1Dto dto);
    Task<int> SaveStep2Async(PatientStep2Dto dto);
    Task<bool> CompleteRegistrationAsync(PasswordStep3Dto dto);
    Task<FamilyRegistrationCache?> ResumeRegistrationAsync(string token);
}
