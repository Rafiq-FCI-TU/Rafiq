using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Common
{
    public class FamilyProfile : ApplicationUser
    {
        public Relationship Relationship { get; set; }  // Family responsible data e.g., Father, Mother
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();

        /*  // Family-specific methods
          public int GetPatientsCount() => Patients?.Count ?? 0;

          public bool HasActivePatients() => Patients?.Any(p => p.DateOfBirth > DateTime.Now.AddYears(-18)) ?? false;*/
    }
}
