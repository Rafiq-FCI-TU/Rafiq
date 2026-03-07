using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;

public interface ISpecialistRegistrationService
{
    Task<Result<StepResponseDto>> RegisterSpecialistStep1Async(SpecialistStep1Dto dto);
    Task<Result> CompleteSpecialistRegistrationAsync(PasswordStepDto dto);
    Task<Result<SpecialistRegistrationCache>> ResumeRegistrationAsync(string token);
}