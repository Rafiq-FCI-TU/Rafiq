using Rafiq.Application.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace Rafiq.Application.Dtos.StepsDtos;
public class PatientStep2Dto
{
    [Required(ErrorMessage = "Registration token is required.")]
    public string Token { get; init; } = null!;

    [Required(ErrorMessage = "Patient first name is required.")]
    [MinLength(2, ErrorMessage = "Patient first name must be at least 2 characters.")]
    [MaxLength(50, ErrorMessage = "Patient first name must be at most 50 characters.")]
    public string FirstName { get; init; } = null!;

    [Required(ErrorMessage = "Patient last name is required.")]
    [MinLength(2, ErrorMessage = "Patient last name must be at least 2 characters.")]
    [MaxLength(50, ErrorMessage = "Patient last name must be at most 50 characters.")]
    public string LastName { get; init; } = null!;

    [Required(ErrorMessage = "Date of birth is required.")]
    [ValidDateOfBirth]
    public DateTime DateOfBirth { get; init; }

    [Required(ErrorMessage = "Gender is required.")]
    public string Gender { get; init; } = null!;
}