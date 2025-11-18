using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;
public class Patient: BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }

    // Foreign key to FamilyProfile
    public string FamilyProfileId { get; set; } = string.Empty;
    public FamilyProfile FamilyProfile { get; set; } = null!;

    // Navigation properties Many-to-many: Multiple specialists
    public ICollection<SpecialistPatient> SpecialistPatients { get; set; } = [];
    public Assessment Assessment { get; set; } = null!; // One-to-One with Assessment 
}
