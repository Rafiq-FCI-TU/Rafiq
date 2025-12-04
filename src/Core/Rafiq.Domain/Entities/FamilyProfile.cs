using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;
public class FamilyProfile : ApplicationUser
{
    public Relationship Relationship { get; set; }  // Family responsible data e.g., Father, Mother
    public string EmergencyContactName { get; set; } = string.Empty;
    public string EmergencyContactPhone { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
}