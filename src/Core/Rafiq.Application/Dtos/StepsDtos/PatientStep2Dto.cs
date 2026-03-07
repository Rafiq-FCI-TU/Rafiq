using Rafiq.Application.CustomValidations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Rafiq.Application.Dtos.StepsDtos;

/// <summary>
/// Step 2 of the registration process.
/// Collects the patient's basic personal information.
/// </summary>
public class PatientStep2Dto
{
    /// <summary>
    /// Temporary registration token returned from Step 1.
    /// Used to continue the multi-step registration process.
    /// </summary>
    /// <example>pending_family_registrati:onc3f8a2f1-8b7d-4c9a-a4b7-0d8d3c9a7e1f</example>
    [Required(ErrorMessage = "Registration token is required.")]
    public string Token { get; init; } = null!;

    /// <summary>
    /// First name of the patient (child).
    /// </summary>
    /// <example>Omar</example>
    [Required(ErrorMessage = "Patient first name is required.")]
    [MinLength(2, ErrorMessage = "Patient first name must be at least 2 characters.")]
    [MaxLength(50, ErrorMessage = "Patient first name must be at most 50 characters.")]
    public string FirstName { get; init; } = null!;

    /// <summary>
    /// Last name of the patient (child).
    /// </summary>
    /// <example>Ahmed</example>
    [Required(ErrorMessage = "Patient last name is required.")]
    [MinLength(2, ErrorMessage = "Patient last name must be at least 2 characters.")]
    [MaxLength(50, ErrorMessage = "Patient last name must be at most 50 characters.")]
    public string LastName { get; init; } = null!;

    /// <summary>
    /// Date of birth of the patient.
    /// Must be a valid past date.
    /// </summary>
    /// <example>2018-05-10</example>
    [Required(ErrorMessage = "Date of birth is required.")]
    [ValidDateOfBirth]
    public DateTime DateOfBirth { get; init; }

    /// <summary>
    /// Gender of the patient.
    /// </summary>
    /// <example>Male</example>
    [Required(ErrorMessage = "Gender is required.")]
    [SwaggerSchema(Description = "Allowed values: Male, Female")]
    public string Gender { get; init; } = null!;
}