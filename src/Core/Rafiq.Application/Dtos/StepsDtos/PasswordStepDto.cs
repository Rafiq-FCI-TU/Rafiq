using System.ComponentModel.DataAnnotations;

namespace Rafiq.Application.Dtos.StepsDtos;
public class PasswordStepDto
{
    [Required(ErrorMessage = "Registration token is required.")]
    public string Token { get; init; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must include uppercase, lowercase, number, and special character."
    )]
    public string Password { get; init; } = null!;

    [Required(ErrorMessage = "Confirm password is required.")]
    [Compare(nameof(Password), ErrorMessage = "Password and confirm password do not match.")]
    public string ConfirmPassword { get; init; } = null!;
}