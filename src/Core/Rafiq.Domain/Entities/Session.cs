using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities
{
    public class Session: BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public int Score { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;

        // Relation with SpecialistProfile
        public string SpecialistProfileId { get; set; } = string.Empty;
        public SpecialistProfile Specialist { get; set; } = null!;
    }

}
