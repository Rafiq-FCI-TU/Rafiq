using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Rafiq.Application.Dtos.StepsDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;

namespace Rafiq.Infrastructure.Services.Auth;

public class FamilyRegistrationService(UserManager<ApplicationUser> userManager, IRedisCacheService cache, IUnitOfWork unitOfWork, IMapper mapper) : IFamilyRegistrationService
{
    public async Task<(string Token, int NextStep)> SaveStep1Async(FamilyStep1Dto dto)
    {
        var token = "pending_registration:" + Guid.NewGuid();
        var cacheModel = new FamilyRegistrationCache
        {
            Email = dto.Email,
            FamilyData = dto,
            CurrentStep = 1
        };

        await cache.SetAsync(token, cacheModel, TimeSpan.FromHours(24));

        return (token, 2);
    }

    public async Task<(string Token, int NextStep)> SaveStep2Async(PatientStep2Dto dto) // FamilyStep2Dto
    {
        // form one token
        var key = dto.Token;

        // form one data
        var cacheModel = await cache.GetAsync<FamilyRegistrationCache>(key);

        if (cacheModel is null)
            throw new InvalidOperationException("Registration session expired or not found.");

        if (cacheModel.CurrentStep != 1)
            throw new InvalidOperationException("Invalid registration session.");

        cacheModel.PatientData = dto;
        cacheModel.CurrentStep = 2;

        await cache.SetAsync(key, cacheModel, TimeSpan.FromHours(24));

        return (key, 3);
    }

    public async Task<bool> CompleteRegistrationAsync(PasswordStep3Dto dto)
    {
        var key = dto.Token;
        var cacheModel = await cache.GetAsync<FamilyRegistrationCache>(key);

        if (cacheModel is null)
            throw new InvalidOperationException("Registration session expired or not found.");

        if (cacheModel.CurrentStep != 2)
            throw new InvalidOperationException("Invalid registration session.");

        List<string> rightRelations = ["Father", "Mother", "Other"];

        if (!rightRelations.Contains(cacheModel.FamilyData.Relationship))
            throw new InvalidOperationException("Invalid relationship type.");

        // CREATE FAMILY PROFILE (Identity User)
        var familyProfile = new FamilyProfile
        {
            UserName = cacheModel.FamilyData.Email.Split('@')[0],
            Email = cacheModel.FamilyData.Email,
            FirstName = cacheModel.FamilyData.FirstName,
            LastName = cacheModel.FamilyData.LastName,
            Relationship = Enum.Parse<Relationship>(cacheModel.FamilyData.Relationship),
            EmergencyContactName = cacheModel.FamilyData.EmergencyContactName,
            EmergencyContactPhone = cacheModel.FamilyData.EmergencyContactPhone
        };

        var createResult = await userManager.CreateAsync(familyProfile, dto.Password);

        if (!createResult.Succeeded)
            throw new Exception("Failed to create user account.");

        // cacheModel.PatientData (validate is exist)
        // cacheModel.PatientData.Gender

        // CREATE PATIENT (child)
        var patient = new Patient
        {
            FirstName = cacheModel.PatientData.FirstName,
            LastName = cacheModel.PatientData.LastName,
            DateOfBirth = cacheModel.PatientData.DateOfBirth,
            Gender = Enum.Parse<Gender>(cacheModel.PatientData.Gender),
            FamilyProfileId = familyProfile.Id
        };

        // Save patient
        await unitOfWork.Repository<Patient>().AddAsync(patient);
        await unitOfWork.CompleteAsync();

        // CLEAR CACHE
        await cache.RemoveAsync(key);

        return true;
    }


    public async Task<FamilyRegistrationCache?> ResumeRegistrationAsync(string token)
    {
        return await cache.GetAsync<FamilyRegistrationCache>(token);
    }
}
