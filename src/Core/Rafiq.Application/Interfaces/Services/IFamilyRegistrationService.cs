using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.ErrorHandling;

public interface IFamilyRegistrationService
{
    Task<Result<StepResponseDto>> RegisterFamilyStep1Async(FamilyStep1Dto dto);
    Task<Result<StepResponseDto>> RegisterFamilyStep2Async(PatientStep2Dto dto);
    Task<Result> CompleteFamilyRegistrationAsync(PasswordStep3Dto dto);
    Task<Result<FamilyRegistrationCache>> ResumeRegistrationAsync(string token);
}
