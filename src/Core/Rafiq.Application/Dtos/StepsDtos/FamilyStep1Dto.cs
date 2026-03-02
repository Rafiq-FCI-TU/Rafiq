using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Rafiq.Application.Dtos.StepsDtos;
public class FamilyStep1Dto
{
    [Required(ErrorMessage = "First name is required.")]
    [MinLength(2, ErrorMessage = "First name must be greeter than 2 characters."), MaxLength(50, ErrorMessage = "First name must be less than 50 characters.")]
    public string FirstName { get; init; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [MinLength(2, ErrorMessage = "Last name must be greeter than 2 characters."), MaxLength(50, ErrorMessage = "Last name must be less than 50 characters.")]
    public string LastName { get; init; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; init; } = null!;

    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^(\+?\d{10,15})$", ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; init; } = null!;
    
    [Required(ErrorMessage = "Relationship is required.")]
    [SwaggerSchema(Description = "Allowed values: Father, Mother, Other")]
    public string Relationship { get; init; } = null!;

    [Required(ErrorMessage = "Address is required.")]
    [MinLength(5, ErrorMessage = "Address must be greeter than 5 characters."), MaxLength(100, ErrorMessage = "Address must be less than 100 characters.")]
    public string Address { get; init; } = null!;

    [Required(ErrorMessage = "Emergency contact name is required.")]
    [MinLength(2, ErrorMessage = "Emergency Contact Name must be greeter than 2 characters."), MaxLength(50, ErrorMessage = "Emergency Contact Name must be less than 50 characters.")]
    public string EmergencyContactName { get; init; } = null!;

    [Required(ErrorMessage = "Emergency contact phone is required.")]
    [RegularExpression(@"^(\+?\d{10,15})$", ErrorMessage = "Invalid emergency contact phone format.")]
    public string EmergencyContactPhone { get; init; } = null!;
}
