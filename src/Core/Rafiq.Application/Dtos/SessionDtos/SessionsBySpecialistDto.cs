namespace Rafiq.Application.Dtos.SessionDtos
{
    public class SessionsBySpecialistDto
    {
            /// <summary>
            /// Unique identifier of the session
            /// </summary>
            /// <example>1</example>
            public int Id { get; set; }

            /// <summary>
            /// Title of the session
            /// </summary>
            /// <example>Introduction to Mindfulness</example>
            public string Title { get; set; } = string.Empty;

            /// <summary>
            /// Detailed description of the session
            /// </summary>
            /// <example>This session covers the basics of mindfulness meditation.</example>
            public string Description { get; set; } = string.Empty;

            /// <summary>
            /// Notes added by the specialist
            /// </summary>
            /// <example>Focus on breathing exercises</example>
            public string Notes { get; set; } = string.Empty;

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
            /// Name of the specialist
            /// </summary>
            /// <example>Dr. Ahmed Ali</example>
            public string SpecialistName { get; set; } = string.Empty;
        }
    }