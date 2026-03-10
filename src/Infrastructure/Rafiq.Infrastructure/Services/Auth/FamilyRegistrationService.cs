using Microsoft.AspNetCore.Identity;
using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services.Auth;
public class FamilyRegistrationService(UserManager<ApplicationUser> userManager, IRedisCacheService cache, IUnitOfWork unitOfWork) : IFamilyRegistrationService
{
    private static readonly TimeSpan RegistrationTtl = TimeSpan.FromHours(24);
    private static string GenerateToken() => $"pending_family_registration:{Guid.NewGuid()}";

    public async Task<Result<StepResponseDto>> RegisterFamilyStep1Async(FamilyStep1Dto dto)
    {
        try
        {
            if (dto is null)
                return Result.Failure<StepResponseDto>(400, "Step 1 data is required.");

            if (!Enum.TryParse<Relationship>(dto.Relationship, true, out _))
                return Result.Failure<StepResponseDto>(400, "Invalid relationship type.");

            var token = GenerateToken();

            var registrationCache = new FamilyRegistrationCache
            {
                Email = dto.Email,
                FamilyData = dto,
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

    public async Task<Result<StepResponseDto>> RegisterFamilyStep2Async(PatientStep2Dto dto)
    {
        try
        {
            if (dto is null)
                return Result.Failure<StepResponseDto>(400, "Patient data is required.");

            if (!Enum.TryParse<Gender>(dto.Gender, true, out _))
                return Result.Failure<StepResponseDto>(400, "Invalid gender type.");

            var registrationCache = await cache.GetAsync<FamilyRegistrationCache>(dto.Token);

            var guard = CheckStepCache(registrationCache, expectedStep: 1);
            if (!guard.IsSuccess)
                return Result.Failure<StepResponseDto>(guard.StatusCode, guard.Message);

            registrationCache!.PatientData = dto;
            registrationCache.CurrentStep = 2;

            await cache.SetAsync(dto.Token, registrationCache, RegistrationTtl);

            return Result.Success(new StepResponseDto
            {
                Token = dto.Token,
                NextStep = 3
            }, "Step 2 completed successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failure<StepResponseDto>(500, $"An unexpected error occurred during step 2, {ex}");
        }
    }

    public async Task<Result<CompleteFamilyRegisterationDto>> CompleteFamilyRegistrationAsync(PasswordStepDto dto)
    {
        try
        {
            if (dto is null) return Result.Failure<CompleteFamilyRegisterationDto>(400, "Password data is required.");

            var registrationCache = await cache.GetAsync<FamilyRegistrationCache>(dto.Token);

            var guard = CheckStepCache(registrationCache, expectedStep: 2);
            if (!guard.IsSuccess)
                return Result.Failure<CompleteFamilyRegisterationDto>(guard.StatusCode, guard.Message);

            if (registrationCache!.PatientData is null)
                return Result.Failure<CompleteFamilyRegisterationDto>(400, "Patient data is missing.");

            var familyResult = await CreateFamilyProfileAsync(registrationCache.FamilyData, dto.Password);

            if (!familyResult.IsSuccess)
                return Result.Failure<CompleteFamilyRegisterationDto>(familyResult.StatusCode, familyResult.Message);

            Enum.TryParse<Gender>(registrationCache.PatientData.Gender, true, out var gender);

            var patient = new Patient
            {
                FirstName = registrationCache.PatientData.FirstName,
                LastName = registrationCache.PatientData.LastName,
                DateOfBirth = registrationCache.PatientData.DateOfBirth,
                Gender = gender,
                FamilyProfileId = familyResult.Value!.Id
            };

            await unitOfWork.Repository<Patient>().AddAsync(patient);
            await unitOfWork.CompleteAsync();

            await cache.RemoveAsync(dto.Token);

            var response = new CompleteFamilyRegisterationDto()
            {
                PatientId = patient.Id
            };

            return Result.Success(response, "Registration completed successfully.");
        }
        catch (Exception)
        {
            return Result.Failure<CompleteFamilyRegisterationDto>(500, "An unexpected error occurred during registration completion.");
        }
    }

    public async Task<Result<FamilyRegistrationCache>> ResumeRegistrationAsync(string token)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token))
                return Result.Failure<FamilyRegistrationCache>(400, "Token is required.");

            var cacheData = await cache.GetAsync<FamilyRegistrationCache>(token);

            if (cacheData is null)
                return Result.Failure<FamilyRegistrationCache>(404, "Session not found.");

            return Result.Success(cacheData, "Session restored successfully.");
        }
        catch (Exception ex)
        {
            return Result.Failure<FamilyRegistrationCache>(500, $"Failed to resume registration, {ex}");
        }
    }

    #region --- Helpers & Guards ---

    private static Result CheckStepCache(FamilyRegistrationCache? cache, int expectedStep)
    {
        if (cache is null)
            return Result.NotFound("Registration session expired.");

        if (cache.CurrentStep != expectedStep)
            return Result.Failure(400, "Invalid registration step order.");

        if (cache.FamilyData is null)
            return Result.Failure(400, "Incomplete registration data.");

        return Result.Success();
    }

    private async Task<Result<FamilyProfile>> CreateFamilyProfileAsync(FamilyStep1Dto dto, string password)
    {
        try
        {
            var profile = new FamilyProfile
            {
                UserName = dto.Email.Split('@')[0],
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Role = UserRole.Family,
                Relationship = Enum.Parse<Relationship>(dto.Relationship, true),
                EmergencyContactName = dto.EmergencyContactName,
                EmergencyContactPhone = dto.EmergencyContactPhone
            };

            var result = await userManager.CreateAsync(profile, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(" | ", result.Errors.Select(e => e.Description));

                return Result.Failure<FamilyProfile>(400, errors);
            }

            var assignRole = await userManager.AddToRoleAsync(profile, nameof(UserRole.Family));

            if (!assignRole.Succeeded)
            {
                var errors = string.Join(" | ", assignRole.Errors.Select(e => e.Description));
                return Result.Failure<FamilyProfile>(400, errors);
            }

            return Result.Success(profile);
        }
        catch (Exception ex)
        {
            return Result.Failure<FamilyProfile>(500, $"Failed to create family profile, {ex}");
        }
    }
    #endregion
}