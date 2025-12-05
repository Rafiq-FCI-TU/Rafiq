namespace Rafiq.Domain.Entities;
public class SpecialistProfile : ApplicationUser
{
    public string Credentials { get; set; } = string.Empty; // MD, PhD, MSc, etc.
    public string Specialty { get; set; } = string.Empty;// Pediatrician, Therapist, Psychologist, etc.
    public string Organization { get; set; } = string.Empty;
    public string ProfessionalBio { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<SpecialistPatient> SpecialistPatients { get; set; } = new List<SpecialistPatient>();
    // Each specialist has many sessions
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
