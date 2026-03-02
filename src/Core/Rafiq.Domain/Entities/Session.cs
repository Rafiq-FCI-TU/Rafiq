namespace Rafiq.Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
         
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string VideoUrl { get; set; } = null!;
        public string Duration { get; set; } = null!;
        public int Sequence { get; set; }
        public int Score { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ThumbnailUrl { get; set; } = null!;

        // Relation with SpecialistProfile
        public string SpecialistProfileId { get; set; } = null!;
        public SpecialistProfile Specialist { get; set; } = null!;
    }

}
