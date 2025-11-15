namespace Rafiq.Domain.Entities
{
    public class SpecialistProfile : ApplicationUser
    {
        public string Credentials { get; set; } = string.Empty; // MD, PhD, MSc, etc.
        public string Specialty { get; set; } = string.Empty;// Pediatrician, Therapist, Psychologist, etc.
        public string Organization { get; set; } = string.Empty;
        public string ProfessionalBio { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<SpecialistPatient> SpecialistPatients { get; set; } = new List<SpecialistPatient>();
        ///TODO: Add them in DTO
        /*
                // Specialist-specific methods
                public int GetActivePatientsCount()
                    => SpecialistPatients?.Count(sp => sp.Patient.DateOfBirth > DateTime.Now.AddYears(-18)) ?? 0;
        */
    }

}
