using Rafiq.Domain.Common;
using Rafiq.Domain.Entities;

namespace Rafiq.Domain.Entities
{
    public class Session: BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }

         public int Sequence { get; set; }
        public int Score { get; set; }
        public DateTime PublishedAt { get; set; }= DateTime.Now;   
        public string ThumbnailUrl { get; set; } = null!;

        // Relation with SpecialistProfile
        public string SpecialistProfileId { get; set; } = null!;
        public SpecialistProfile Specialist { get; set; } = null!;
    }

}
// Rafiq.Domain.Entities/PatientSessionAttempt.cs


public class PatientSessionAttempt : BaseEntity
{
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;

    public string VideoUrl { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }

    public AttemptStatus Status { get; set; } = AttemptStatus.PendingReview;

    // Doctor's feedback
    public string? DoctorFeedback { get; set; }
    public int? ScoreAwarded { get; set; } // الـ score اللي الدكتور يديه

    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
}
public enum AttemptStatus
{
    PendingReview,
    Approved,
    Rejected
}