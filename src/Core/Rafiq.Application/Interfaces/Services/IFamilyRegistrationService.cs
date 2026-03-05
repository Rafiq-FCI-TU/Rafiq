using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;

public interface IFamilyRegistrationService
{
    Task<Result<StepResponseDto>> RegisterFamilyStep1Async(FamilyStep1Dto dto);
    Task<Result<StepResponseDto>> RegisterFamilyStep2Async(PatientStep2Dto dto);
    Task<Result> CompleteFamilyRegistrationAsync(PasswordStepDto dto);
    Task<Result<FamilyRegistrationCache>> ResumeRegistrationAsync(string token);
}