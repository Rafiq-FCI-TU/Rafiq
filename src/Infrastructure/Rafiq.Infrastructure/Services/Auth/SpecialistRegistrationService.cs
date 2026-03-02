using Microsoft.AspNetCore.Identity;
using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services.Auth;
public class SpecialistRegistrationService(UserManager<ApplicationUser> userManager, IRedisCacheService cache, IUnitOfWork unitOfWork) : ISpecialistRegistrationService
{
    private static readonly TimeSpan RegistrationTtl = TimeSpan.FromHours(24);
    private static string GenerateToken() => $"pending_specialist_registration:{Guid.NewGuid()}";

    public async Task<Result<StepResponseDto>> RegisterSpecialistStep1Async(SpecialistStep1Dto dto)
    {
        try
        {
            if (dto is null)
                return Result.Failure<StepResponseDto>(400, "Specialist registration step 1 data is required.");

            if (!Enum.TryParse<Gender>(dto.Gender, true, out _))
                return Result.Failure<StepResponseDto>(400, "Invalid gender type.");

            var token = GenerateToken();

            var registrationCache = new SpecialistRegistrationCache
            {
                Email = dto.Email,
                SpecialistData = dto,
                CurrentStep = 1
            };

            await cache.SetAsync(token, registrationCache, RegistrationTtl);

            return Result.Success(new StepResponseDto
            {
                Token = token,
                NextStep = 2
            }, "Step 1 completed successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failure<StepResponseDto>(500, $"An unexpected error occurred during step 1, {ex}");
        }
    }

    public async Task<Result> CompleteSpecialistRegistrationAsync(PasswordStepDto dto)
    {
        try
        {
            if (dto is null)
                return Result.BadRequest("Specialist registration password data is missed.");

            var registrationCache = await cache.GetAsync<SpecialistRegistrationCache>(dto.Token);

            var guard = CheckStepCache(registrationCache, expectedStep: 1);
            if (!guard.IsSuccess)
                return guard;

            var specialistData = registrationCache!.SpecialistData!;

            if (!Enum.TryParse<Gender>(specialistData.Gender, true, out var gender))
                return Result.Failure(400, "Invalid gender type.");

            var specialist = new SpecialistProfile
            {
                UserName = specialistData.Email.Split('@')[0],
                Email = specialistData.Email,
                FirstName = specialistData.FirstName,
                LastName = specialistData.LastName,
                Gender = gender.ToString(),
                Role = UserRole.Specialist,
                Credentials = specialistData.Credentials,
                Specialty = specialistData.Specialty,
                Organization = specialistData.Organization,
                ProfessionalBio = specialistData.ProfessionalBio
            };

            var result = await userManager.CreateAsync(specialist, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result.Failure(400, $"Failed to create specialist account: {errors}");
            }

            var assignmentResult = await userManager.AddToRoleAsync(specialist, nameof(UserRole.Specialist));

            if (!assignmentResult.Succeeded)
            {
                var errors = string.Join(" | ", assignmentResult.Errors.Select(e => e.Description));
                return Result.Failure(500, $"Specialist account created but failed to assign role: {errors}");
            }

            await cache.RemoveAsync(dto.Token);

            return Result.Success("Specialist registration completed successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failure(500, $"An unexpected error occurred during specialist registration, {ex}");
        }
    }

    public async Task<Result<SpecialistRegistrationCache>> ResumeRegistrationAsync(string token)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token))
                return Result.Failure<SpecialistRegistrationCache>(400, "Token is required.");

            var registrationCache = await cache.GetAsync<SpecialistRegistrationCache>(token);

            if (registrationCache is null)
            {
                return Result.Failure<SpecialistRegistrationCache>(404, "Registration session not found.");
            }

            return Result.Success(registrationCache, "Registration session restored successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failure<SpecialistRegistrationCache>(500, $"Failed to resume registration, {ex}");
        }
    }

    #region --- Helpers & Guards ---

    private static Result CheckStepCache(SpecialistRegistrationCache? cache, int expectedStep)
    {
        if (cache is null)
            return Result.NotFound("Registration session expired.");

        if (cache.CurrentStep != expectedStep)
            return Result.Failure(400, "Invalid registration step order.");

        if (cache.SpecialistData is null)
            return Result.Failure(400, "Incomplete registration data.");

        return Result.Success();
    }

    #endregion
}
