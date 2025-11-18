using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Rafiq.Application.Dtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Common;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;

namespace Rafiq.Infrastructure.Services.Auth;

public class FamilyRegistrationService(UserManager<ApplicationUser> _userManager, IRedisCacheService _cache, IUnitOfWork _unitOfWork, IMapper _mapper) : IFamilyRegistrationService
{
    private const string CacheKeyPrefix = "pending_registration:";

    public async Task<(string Token, int NextStep)> SaveStep1Async(FamilyStep1Dto dto)
    {
        var token = Guid.NewGuid().ToString();
        var cacheModel = new FamilyRegistrationCache
        {
            Email = dto.Email,
            FamilyData = dto,
            CurrentStep = 1
        };

        await _cache.SetAsync(CacheKeyPrefix + token, cacheModel, TimeSpan.FromHours(24));

        return (token, 2);
    }

    public async Task<int> SaveStep2Async(PatientStep2Dto dto)
    {
        var key = CacheKeyPrefix + dto.Token;

        var cacheModel = await _cache.GetAsync<FamilyRegistrationCache>(key);

        if (cacheModel is null)
            throw new InvalidOperationException("Registration session expired or not found.");

        cacheModel.PatientData = dto;
        cacheModel.CurrentStep = 2;

        await _cache.SetAsync(key, cacheModel, TimeSpan.FromHours(24));

        return 3;
    }

    public async Task<bool> CompleteRegistrationAsync(PasswordStep3Dto dto)
    {
        var key = CacheKeyPrefix + dto.Token;
        var data = await _cache.GetAsync<FamilyRegistrationCache>(key);

        if (data is null)
            throw new InvalidOperationException("Registration session expired or not found.");

        // CREATE FAMILY PROFILE (Identity User)
        var familyProfile = new FamilyProfile
        {
            UserName = data.Email,
            Email = data.Email,
            FirstName = data.FamilyData.FirstName,
            LastName = data.FamilyData.LastName,
            Relationship = Enum.Parse<Relationship>(data.FamilyData.Relationship),
            EmergencyContactName = data.FamilyData.EmergencyContactName,
            EmergencyContactPhone = data.FamilyData.EmergencyContactPhone
        };

        var createResult = await _userManager.CreateAsync(familyProfile, dto.Password);

        if (!createResult.Succeeded)
            throw new Exception("Failed to create user account.");

        // CREATE PATIENT (child)
        var patient = new Patient
        {
            FirstName = data.PatientData.FirstName,
            LastName = data.PatientData.LastName,
            DateOfBirth = data.PatientData.DateOfBirth,
            Gender = Enum.Parse<Gender>(data.PatientData.Gender),
            FamilyProfileId = familyProfile.Id
        };
        // Save patient
        await _unitOfWork.Repository<Patient>().AddAsync(patient);
        await _unitOfWork.CompleteAsync();

        // CLEAR CACHE
        await _cache.RemoveAsync(key);

        return true;
    }


    public async Task<FamilyRegistrationCache?> ResumeRegistrationAsync(string token)
    {
        return await _cache.GetAsync<FamilyRegistrationCache>(CacheKeyPrefix + token);
    }
}
