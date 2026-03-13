using Microsoft.AspNetCore.Http;

namespace Rafiq.Application.Dtos.SessionDtos
{
    /// <summary>
    /// DTO for creating a new session.
    /// </summary>
    public class SessionCreateDto
    {
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
        /// Main media file for the session (video/audio)
        /// </summary>
        /// <example>session1.mp4</example>
        /// 
        public IFormFile Media { get; set; } = null!;

        /// <summary>
        /// Optional thumbnail image for the session
        /// </summary>
        /// <example>thumb1.png</example>
        public IFormFile? Thumbnail { get; set; } = null;

        /// <summary>
        /// Sequence order of the session
        /// </summary>
        /// <example>1</example>
        //public int Sequence { get; set; }

        /// <summary>
        /// Score or rating associated with the session
        /// </summary>
        /// <example>100</example>
        public int Score { get; set; }


        /// <summary>
        /// Specialist profile ID who is creating the session
        /// </summary>
        /// <example>sp12345</example>
        public string SpecialistProfileId { get; set; } = string.Empty;
    }
}