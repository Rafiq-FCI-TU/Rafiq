using Microsoft.AspNetCore.Identity;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public UserRole Role { get; set; } 
    public UserStatus Status { get; set; } = UserStatus.Active;
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public List<RefreshToken>? RefreshTokens { get; set; }
}
