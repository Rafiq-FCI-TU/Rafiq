namespace Rafiq.Application.Dtos.SessionDtos
{
    /// <summary>
    /// Represents a session visible to a specific patient.
    /// </summary>
    public class Sessionfor_Specialist_patientDto
    {
        /// <summary>
        /// Unique identifier of the session
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Notes added by the specialist for this patient
        /// </summary>
        /// <example>Focus on posture correction</example>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// Title of the session
        /// </summary>
        /// <example>Introduction to Mindfulness</example>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the session
        /// </summary>
        /// <example>This session covers basic mindfulness exercises for stress reduction.</example>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// URL of the session video
        /// </summary>
        /// <example>https://videos.example.com/session1.mp4</example>
        public string VideoUrl { get; set; } = string.Empty;

        /// <summary>
        /// Duration of the session in minutes
        /// </summary>
        /// <example>45 mins</example>
        public string Duration { get; set; } = string.Empty;

        /// <summary>
        /// Sequence order of the session
        /// </summary>
        /// <example>1</example>
        public int Sequence { get; set; }

        /// <summary>
        /// Score or rating associated with the session
        /// </summary>
        /// <example>100</example>
        public int Score { get; set; }

        /// <summary>
        /// Date and time when the session was published
        /// </summary>
        /// <example>2026-03-08T14:30:00Z</example>
        public DateTime PublishedAt { get; set; }

        /// <summary>
        /// URL of the thumbnail image
        /// </summary>
        /// <example>https://images.example.com/thumb1.png</example>
        public string ThumbnailUrl { get; set; } = string.Empty;

        /// <summary>
        /// Specialist profile ID who created the session
        /// </summary>
        /// <example>sp12345</example>
        public string SpecialistProfileId { get; set; } = string.Empty;

        /// <summary>
        /// ID of the patient who can access the session
        /// </summary>
        /// <example>p98765</example>
        public int PatientId { get; set; }
    }
}