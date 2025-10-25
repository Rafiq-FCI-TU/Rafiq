using Microsoft.AspNetCore.Identity;
using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities
{
    public abstract class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty; // "Family", "Specialist"
        public UserRole Role { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        //public FamilyProfile? FamilyProfile { get; set; }
        //public SpecialistProfile? SpecialistProfile { get; set; }
    }

}
