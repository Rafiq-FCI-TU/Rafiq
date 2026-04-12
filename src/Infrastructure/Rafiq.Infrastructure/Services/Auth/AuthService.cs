using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Rafiq.Application.Dtos;
using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Application.Specifications;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;
using Rafiq.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Rafiq.Infrastructure.Services.Auth;
public class AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> jwtSettings,IUnitOfWork unitOfWork) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public async Task<Result<ResponseForFamilyRegistrationDto>> LoginAsync(LoginRequestDto model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            return Result.Failure<ResponseForFamilyRegistrationDto>(400, "Email and password are required.");

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user is null || !await userManager.CheckPasswordAsync(user, model.Password))
            return Result.Failure<ResponseForFamilyRegistrationDto>(400, "The email or password you entered is incorrect.");

        CleanupExpiredRefreshTokens(user);

        var jwtToken = await CreateJwtToken(user);
        var roles = await userManager.GetRolesAsync(user);

        var activeRefreshToken = user.RefreshTokens?.FirstOrDefault(t => t.IsActive) ?? GenerateRefreshToken();

        if (user.RefreshTokens?.Any(t => t.IsActive) != true)
        {
            user.RefreshTokens?.Add(activeRefreshToken);
            await userManager.UpdateAsync(user);
        }

        if (roles.Contains("Family"))
        {
            var spec = new BaseSpecifications<Patient>
            {
                WhereCriteria = p => p.FamilyProfileId == user.Id
            };
            spec.AddInclude(nameof(Patient.SpecialistPatients)); 

            var patient = await unitOfWork.Repository<Patient>().GetEntityAsync(spec);

            bool hasAssessment = patient?.AssessmentId > 0;
            int? assessmentId = hasAssessment ? patient?.AssessmentId : null;

            string? hasSpecialist = patient?.SpecialistPatients
                .FirstOrDefault()?.SpecialistId;
            return Result.Success(new ResponseForFamilyRegistrationDto
            {
                Id = user.Id,
                Specialistid = hasSpecialist,     
                Patientid = patient?.Id,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Email = user.Email ?? string.Empty,
                Username = user.UserName ?? string.Empty,
                Roles = roles.ToList(),
                ExpiresOn = jwtToken.ValidTo,
                RefreshToken = activeRefreshToken.Token,
                RefreshTokenExpiration = activeRefreshToken.ExpiresOn,
                HasAssessment = hasAssessment,
                AssessmentId = assessmentId
            });
        }

        return Result.Success(new ResponseForFamilyRegistrationDto
        {
            Id = user.Id,
            Patientid= null,
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            Email = user.Email ?? string.Empty,
            Username = user.UserName ?? string.Empty,
            Roles = roles.ToList(),
            ExpiresOn = jwtToken.ValidTo,
            RefreshToken = activeRefreshToken.Token,
            RefreshTokenExpiration = activeRefreshToken.ExpiresOn
        });
    }

    public async Task<Result<ResponseDto>> RefreshTokenAsync(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Result.Failure<ResponseDto>(400, "Token is required.");

        var user = await userManager.Users
            .SingleOrDefaultAsync(u => u.RefreshTokens != null && u.RefreshTokens.Any(t => t.Token == token));

        if (user is null)
            return Result.Failure<ResponseDto>(401, "Invalid token.");

        var refreshToken = user.RefreshTokens!.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
            return Result.Failure<ResponseDto>(401, "Token is inactive or expired.");

        refreshToken.RevokedOn = DateTime.UtcNow;

        CleanupExpiredRefreshTokens(user);

        var newRefreshToken = GenerateRefreshToken();
        user.RefreshTokens!.Add(newRefreshToken);
        await userManager.UpdateAsync(user);

        var jwtToken = await CreateJwtToken(user);
        var roles = await userManager.GetRolesAsync(user);

        return Result.Success(new ResponseDto
        {
            IsAuthenticated = true,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            Email = user.Email ?? string.Empty,
            Username = user.UserName ?? string.Empty,
            Roles = roles.ToList(),
            ExpiresOn = jwtToken.ValidTo,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpiration = newRefreshToken.ExpiresOn
        });
    }

    public async Task<Result<bool>> RevokeTokenAsync(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Result.Failure<bool>(400, "Token is required.");

        var user = await userManager.Users
            .SingleOrDefaultAsync(u => u.RefreshTokens != null && u.RefreshTokens.Any(t => t.Token == token));

        if (user is null)
            return Result.Failure<bool>(401, "Invalid token.");

        var refreshToken = user.RefreshTokens!.Single(t => t.Token == token);

        if (!refreshToken.IsActive)
            return Result.Failure<bool>(400, "Token is already inactive.");

        refreshToken.RevokedOn = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        return Result.Success(true);
    }

    private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await userManager.GetClaimsAsync(user);
        var roles = await userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new("uid", user.Id)
        };

        claims.AddRange(userClaims);
        claims.AddRange(roles.Select(r => new Claim("roles", r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        return new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInDays),
            signingCredentials: credentials
        );
    }

    private static RefreshToken GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            CreatedOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddDays(7)
        };
    }

    private static void CleanupExpiredRefreshTokens(ApplicationUser user)
    {
        if (user.RefreshTokens is null) return;

        var expiredTokens = user.RefreshTokens
            .Where(t => !t.IsActive)
            .ToList();

        foreach (var expired in expiredTokens)
            user.RefreshTokens.Remove(expired);
    }
}