using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;

public class Gaming : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation properties
    public Patient Patient { get; set; } = null!;
}
