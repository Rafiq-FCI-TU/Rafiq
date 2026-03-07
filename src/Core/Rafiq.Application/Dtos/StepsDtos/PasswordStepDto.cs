using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Rafiq.Application.Dtos.StepsDtos;

/// <summary>
/// Final step of the registration process.
/// Allows the user to set their account password.
/// </summary>
public class PasswordStepDto
{
    /// <summary>
    /// Temporary registration token obtained from previous registration steps.
    /// </summary>
    /// <example>c3f8a2f1-8b7d-4c9a-a4b7-0d8d3c9a7e1f</example>
    [Required(ErrorMessage = "Registration token is required.")]
    public string Token { get; init; } = null!;

    /// <summary>
    /// Account password.
    /// Must contain at least:
    /// - 8 characters
    /// - one uppercase letter
    /// - one lowercase letter
    /// - one number
    /// - one special character
    /// </summary>
    /// <example>Secure@123</example>
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must include uppercase, lowercase, number, and special character."
    )]
    public string Password { get; init; } = null!;

    /// <summary>
    /// Must match the password field.
    /// </summary>
    /// <example>Secure@123</example>
    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare(nameof(Password), ErrorMessage = "Password and confirm password do not match.")]
    public string ConfirmPassword { get; init; } = null!;
}