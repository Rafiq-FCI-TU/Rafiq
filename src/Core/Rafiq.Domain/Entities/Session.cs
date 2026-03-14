using Rafiq.Domain.Common;
namespace Rafiq.Domain.Entities;
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

        public ICollection<PatientSessionAttempt> PatientAttempts { get; set; } = new List<PatientSessionAttempt>();
    }