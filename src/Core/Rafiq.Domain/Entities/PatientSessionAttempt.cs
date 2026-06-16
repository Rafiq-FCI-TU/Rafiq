using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;
namespace Rafiq.Domain.Entities;

public class PatientSessionAttempt : BaseEntity
{
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public string VideoUrl { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }

    public AttemptStatus Status { get; set; } = AttemptStatus.PendingReview;

    public string? DoctorFeedback { get; set; }
    public int? ScoreAwarded { get; set; } 

    public DateTime SubmittedAt { get; set; } = DateTime.Now;
    public DateTime? ReviewedAt { get; set; }
}
