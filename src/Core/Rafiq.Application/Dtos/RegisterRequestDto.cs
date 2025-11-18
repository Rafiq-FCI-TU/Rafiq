using System.ComponentModel.DataAnnotations;

namespace Rafiq.Application.Dtos;

public class RegisterRequestDto
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, StringLength(128)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(256)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string UserType { get; set; } = string.Empty; // "Family" or "Specialist"

    // Optional (Family)
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }

    // Optional (Specialist)
    public string? Credentials { get; set; }
    public string? Specialty { get; set; }
    public string? Organization { get; set; }
}