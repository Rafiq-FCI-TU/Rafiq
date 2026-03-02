using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Rafiq.Application.Dtos.StepsDtos;
public class SpecialistStep1Dto
{
    [Required(ErrorMessage = "First name is required.")]
    [MinLength(2, ErrorMessage = "First name must be at least 2 characters.")]
    [MaxLength(50, ErrorMessage = "First name must not exceed 50 characters.")]
    public string FirstName { get; init; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [MinLength(2, ErrorMessage = "Last name must be at least 2 characters.")]
    [MaxLength(50, ErrorMessage = "Last name must not exceed 50 characters.")]
    public string LastName { get; init; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^(\+?\d{10,15})$", ErrorMessage = "Invalid phone number format.")]
    public string Phone { get; init; } = null!;

    [Required(ErrorMessage = "Credentials are required.")]
    [MinLength(2, ErrorMessage = "Credentials must be at least 2 characters.")]
    [MaxLength(20, ErrorMessage = "Credentials must not exceed 20 characters.")]
    [SwaggerSchema(Description = "Examples: MD, PhD, MSc")]
    public string Credentials { get; set; } = string.Empty;

    [Required(ErrorMessage = "Specialty is required.")]
    [MinLength(2, ErrorMessage = "Specialty must be at least 2 characters.")]
    [MaxLength(50, ErrorMessage = "Specialty must not exceed 50 characters.")]
    [SwaggerSchema(Description = "Examples: Pediatrician, Therapist, Psychologist")]
    public string Specialty { get; set; } = string.Empty;

    [Required(ErrorMessage = "Organization is required.")]
    [MinLength(2, ErrorMessage = "Organization name must be at least 2 characters.")]
    [MaxLength(100, ErrorMessage = "Organization name must not exceed 100 characters.")]
    public string Organization { get; set; } = string.Empty;

    [Required(ErrorMessage = "Professional bio is required.")]
    [MinLength(20, ErrorMessage = "Professional bio must be at least 20 characters.")]
    [MaxLength(1000, ErrorMessage = "Professional bio must not exceed 1000 characters.")]
    public string ProfessionalBio { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required.")]
    [SwaggerSchema(Description = "Allowed values: Male, Female")]
    public string Gender { get; set; } = string.Empty;
}