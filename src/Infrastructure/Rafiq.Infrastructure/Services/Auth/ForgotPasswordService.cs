using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Infrastructure.Services.Auth;
public class ForgotPasswordService(UserManager<ApplicationUser> userManager, IEmailService emailService) : IForgotPasswordService
{
    public async Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordRequestDto model)
    {
        if (string.IsNullOrWhiteSpace(model.Email))
            return Result.Failure<bool>(400, "Email is required.");

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user is null)
            return Result.Success(true);

        CleanupExpiredOtps(user);

        var otp = GenerateOtp();

        user.PasswordResetTokens ??= [];
        user.PasswordResetTokens.Add(new PasswordResetToken
        {
            Otp = otp,
            CreatedOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddMinutes(10)
        });

        await userManager.UpdateAsync(user);

        await emailService.SendAsync(
            toEmail: user.Email!,
            subject: "Reset Your Password",
            body: BuildEmailBody(user.UserName ?? "User", otp)
        );

        return Result.Success(true);
    }

    public async Task<Result<bool>> VerifyOtpAsync(VerifyOtpRequestDto model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Otp))
            return Result.Failure<bool>(400, "Email and OTP are required.");

        var user = await userManager.Users
            .Include(u => u.PasswordResetTokens)
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user is null)
            return Result.Failure<bool>(400, "Invalid request.");

        var resetToken = user.PasswordResetTokens?
            .Where(t => t.IsActive)
            .OrderByDescending(t => t.CreatedOn)
            .FirstOrDefault(t => t.Otp == model.Otp);

        if (resetToken is null)
            return Result.Failure<bool>(400, "Invalid or expired OTP.");

        resetToken.IsVerified = true;
        await userManager.UpdateAsync(user);

        return Result.Success(true);
    }

    public async Task<Result<bool>> ResetPasswordAsync(ResetPasswordRequestDto model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) ||
            string.IsNullOrWhiteSpace(model.Otp) ||
            string.IsNullOrWhiteSpace(model.NewPassword))
            return Result.Failure<bool>(400, "All fields are required.");

        if (model.NewPassword != model.ConfirmPassword)
            return Result.Failure<bool>(400, "Passwords do not match.");

        var user = await userManager.Users
            .Include(u => u.PasswordResetTokens)
            .FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user is null)
            return Result.Failure<bool>(400, "Invalid request.");

        var resetToken = user.PasswordResetTokens?
            .Where(t => t.IsVerified && !t.IsExpired)
            .OrderByDescending(t => t.CreatedOn)
            .FirstOrDefault(t => t.Otp == model.Otp);

        if (resetToken is null)
            return Result.Failure<bool>(400, "OTP not verified or has expired. Please request a new one.");

        var resetPasswordToken = await userManager.GeneratePasswordResetTokenAsync(user);
        var identityResult = await userManager.ResetPasswordAsync(user, resetPasswordToken, model.NewPassword);

        if (!identityResult.Succeeded)
        {
            var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            return Result.Failure<bool>(400, errors);
        }

        user.PasswordResetTokens?.Clear();
        await userManager.UpdateAsync(user);

        return Result.Success(true);
    }

    private static string GenerateOtp()
    {
        var bytes = new byte[4];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        var number = BitConverter.ToUInt32(bytes, 0) % 1_000_000;
        return number.ToString("D6");
    }

    private static void CleanupExpiredOtps(ApplicationUser user)
    {
        if (user.PasswordResetTokens is null) return;

        var expired = user.PasswordResetTokens
            .Where(t => t.IsExpired)
            .ToList();

        foreach (var token in expired)
            user.PasswordResetTokens.Remove(token);
    }

    private static string BuildEmailBody(string username, string otp) => $"""
        <div style="font-family: Arial, sans-serif; max-width: 480px; margin: auto; padding: 24px; border: 1px solid #e0e0e0; border-radius: 8px;">
            <h2 style="color: #333;">Password Reset Request</h2>
            <p>Hi <strong>{username}</strong>,</p>
            <p>Use the OTP below to reset your password. It will expire in <strong>10 minutes</strong>.</p>
            <div style="text-align: center; margin: 32px 0;">
                <span style="font-size: 36px; font-weight: bold; letter-spacing: 10px; color: #4F46E5;">
                    {otp}
                </span>
            </div>
            <p style="color: #888; font-size: 13px;">If you didn't request this, you can safely ignore this email.</p>
        </div>
        """;
}
