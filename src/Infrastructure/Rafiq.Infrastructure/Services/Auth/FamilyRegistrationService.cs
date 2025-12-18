using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services.Auth;

public class FamilyRegistrationService(
    UserManager<ApplicationUser> userManager,
    IRedisCacheService cache,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IFamilyRegistrationService
{
    private static readonly List<string> AllowedRelationships = new() { "Father", "Mother", "Other" };

    // -------------------- Step 1 --------------------
    public async Task<Result<StepResponseDto>> RegisterFamilyStep1Async(FamilyStep1Dto dto)
    {
        var validationResult = ValidateStep1Dto(dto);
        if (!validationResult.IsSuccess)
            return Result.Failure<StepResponseDto>(400, "Step 1 data cannot be null.");

        var token = GenerateToken();

        var registrationCache = new FamilyRegistrationCache
        {
            Email = dto.Email,
            FamilyData = dto,
            CurrentStep = 1
        };
        var response = new StepResponseDto
        {
            Token = token,
            NextStep = 2
        };  

        await cache.SetAsync(token, registrationCache, TimeSpan.FromHours(24));

        return Result.Success(response, "Step 1 completed successfully.");
    }

    // -------------------- Step 2 --------------------
    public async Task<Result<StepResponseDto>> RegisterFamilyStep2Async(PatientStep2Dto dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Token))
            return Result.Failure<StepResponseDto>(400, "Step 2 data or token cannot be null.");

        var registrationCache = await cache.GetAsync<FamilyRegistrationCache>(dto.Token);
        var guardResult = CheckStepCache(registrationCache,expectedStep: 1);
        if (!guardResult.IsSuccess)
            return Result.Failure<StepResponseDto>(guardResult.StatusCode, guardResult.Message);
        if (AllowedRelationships.Contains(dto.Gender))
            return Result.Failure<StepResponseDto>(400, "Invalid gender type.");

        registrationCache.PatientData = dto;
        registrationCache.CurrentStep = 2;

        var response = new StepResponseDto
        {
            Token = dto.Token,
            NextStep = 3
        };
        
        await cache.SetAsync(dto.Token, registrationCache, TimeSpan.FromHours(24));

        return Result.Success(response, "Step 2 completed successfully.");
    }


    // -------------------- Step 3 --------------------
    public async Task<Result> CompleteFamilyRegistrationAsync(PasswordStep3Dto dto)
    {
        if (dto is null || string.IsNullOrWhiteSpace(dto.Token))
            return Result.BadRequest("Step 3 data or token cannot be null.");

        var registrationCache = await cache.GetAsync<FamilyRegistrationCache>(dto.Token);
        var guardResult = CheckStepCache(registrationCache, expectedStep: 2);
        if (!guardResult.IsSuccess)
            return guardResult;

        // Parse enums safely after guard checks passed 
        Enum.TryParse<Relationship>(registrationCache.FamilyData.Relationship, true, out var relationship);
        Enum.TryParse<Gender>(registrationCache.PatientData.Gender, true, out var gender);

        var userCreationResult = await CreateFamilyProfileAsync(registrationCache.FamilyData, dto.Password);
        if (!userCreationResult.IsSuccess)
            return userCreationResult;

        var familyProfile = userCreationResult.Value;

        var patient = new Patient
        {
            FirstName = registrationCache.PatientData.FirstName,
            LastName = registrationCache.PatientData.LastName,
            DateOfBirth = registrationCache.PatientData.DateOfBirth,
            Gender = gender,
            FamilyProfileId = familyProfile.Id
        };

        await unitOfWork.Repository<Patient>().AddAsync(patient);
        await unitOfWork.CompleteAsync();

        await cache.RemoveAsync(dto.Token);

        return Result.Success("Registration completed successfully.");
    }

    // -------------------- Resume --------------------
    public async Task<Result<FamilyRegistrationCache>> ResumeRegistrationAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Result.Failure<FamilyRegistrationCache>(400, "Token cannot be null.");

        var registrationCache = await cache.GetAsync<FamilyRegistrationCache>(token);
        if (registrationCache is null)
            return Result.Failure<FamilyRegistrationCache>(404,"Registration session not found.");

        return Result.Success(registrationCache, "Registration session retrieved successfully.");
    }

    #region --- Helpers & Guards ---

    private static string GenerateToken() => $"pending_registration:{Guid.NewGuid()}";

    private static Result ValidateStep1Dto(FamilyStep1Dto dto)
    {
        if (dto is null)
            return Result.Failure<(string, int)>(400, "Step 1 data cannot be null.");

        if (string.IsNullOrWhiteSpace(dto.Email))
            return Result.Failure<(string, int)>(400, "Email is required.");

        if (!AllowedRelationships.Contains(dto.Relationship))
            return Result.Failure<(string, int)>(400, "Invalid relationship type.");

        return Result.Success();
    }

    private static Result CheckStepCache(FamilyRegistrationCache? cache, int expectedStep)
    {
        if (cache is null)
            return Result.NotFound("Registration session not found or expired.");

        if (cache.CurrentStep != expectedStep)
            return Result.Failure(400, "Invalid registration step sequence.");

        if (cache.FamilyData is null)
            return Result.Failure(400, "Incomplete registration data.");

        if (!Enum.TryParse<Relationship>(cache.FamilyData.Relationship, true, out _))
            return Result.Failure(400, "Invalid relationship type.");

        return Result.Success();
    }

    private async Task<Result<FamilyProfile>> CreateFamilyProfileAsync(FamilyStep1Dto familyDto, string password)
    {
        var profile = new FamilyProfile
        {
            UserName = familyDto.Email.Split('@')[0],
            Email = familyDto.Email,
            FirstName = familyDto.FirstName,
            LastName = familyDto.LastName,
            Relationship = Enum.Parse<Relationship>(familyDto.Relationship),
            EmergencyContactName = familyDto.EmergencyContactName,
            EmergencyContactPhone = familyDto.EmergencyContactPhone
        };

        var result = await userManager.CreateAsync(profile, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            return Result.Failure<FamilyProfile>(400, $"Failed to create user account: {errors}");
        }

        return Result.Success(profile);
    }
    #endregion
}
