namespace Rafiq.Domain.Entities;
public class SpecialistPatient
{
    public string SpecialistId { get; set; } = string.Empty;
    public SpecialistProfile Specialist { get; set; } = null!;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public DateTime AssignedAt { get; set; }
}
