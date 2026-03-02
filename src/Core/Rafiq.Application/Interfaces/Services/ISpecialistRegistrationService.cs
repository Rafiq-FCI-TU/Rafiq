using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.ErrorHandling;

public interface ISpecialistRegistrationService
{
    Task<Result<StepResponseDto>> RegisterSpecialistStep1Async(SpecialistStep1Dto dto);
    Task<Result> CompleteSpecialistRegistrationAsync(PasswordStepDto dto);
    Task<Result<SpecialistRegistrationCache>> ResumeRegistrationAsync(string token);
}
