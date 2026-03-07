using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Rafiq.Application.Dtos.StepsDtos;

/// <summary>
/// Step 1 of family registration.
/// Collects the primary guardian's basic information.
/// </summary>
public class FamilyStep1Dto
{
    /// <summary>
    /// First name of the parent or guardian.
    /// </summary>
    /// <example>Ahmed</example>
    [Required(ErrorMessage = "First name is required.")]
    [MinLength(2, ErrorMessage = "First name must be greater than 2 characters.")]
    [MaxLength(50, ErrorMessage = "First name must be less than 50 characters.")]
    public string FirstName { get; init; } = null!;

    /// <summary>
    /// Last name of the parent or guardian.
    /// </summary>
    /// <example>Ali</example>
    [Required(ErrorMessage = "Last name is required.")]
    [MinLength(2, ErrorMessage = "Last name must be greater than 2 characters.")]
    [MaxLength(50, ErrorMessage = "Last name must be less than 50 characters.")]
    public string LastName { get; init; } = null!;

    /// <summary>
    /// Email address used for login and communication.
    /// </summary>
    /// <example>parent@example.com</example>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; init; } = null!;

    /// <summary>
    /// Phone number of the parent or guardian.
    /// Must be between 10 and 15 digits and may include country code.
    /// </summary>
    /// <example>+201234567890</example>
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^(\+?\d{10,15})$", ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; init; } = null!;

    /// <summary>
    /// Relationship of the user to the child.
    /// </summary>
    /// <example>Father</example>
    [Required(ErrorMessage = "Relationship is required.")]
    [SwaggerSchema(Description = "Allowed values: Father, Mother, Other")]
    public string Relationship { get; init; } = null!;

    /// <summary>
    /// Home address of the parent or guardian.
    /// </summary>
    /// <example>12 El-Galaa Street, Tanta</example>
    [Required(ErrorMessage = "Address is required.")]
    [MinLength(5, ErrorMessage = "Address must be greater than 5 characters.")]
    [MaxLength(100, ErrorMessage = "Address must be less than 100 characters.")]
    public string Address { get; init; } = null!;

    /// <summary>
    /// Name of the emergency contact person.
    /// </summary>
    /// <example>Mohamed Hassan</example>
    [Required(ErrorMessage = "Emergency contact name is required.")]
    [MinLength(2, ErrorMessage = "Emergency Contact Name must be greater than 2 characters.")]
    [MaxLength(50, ErrorMessage = "Emergency Contact Name must be less than 50 characters.")]
    public string EmergencyContactName { get; init; } = null!;

    /// <summary>
    /// Emergency contact phone number.
    /// </summary>
    /// <example>+201112223334</example>
    [Required(ErrorMessage = "Emergency contact phone is required.")]
    [RegularExpression(@"^(\+?\d{10,15})$", ErrorMessage = "Invalid emergency contact phone format.")]
    public string EmergencyContactPhone { get; init; } = null!;
}