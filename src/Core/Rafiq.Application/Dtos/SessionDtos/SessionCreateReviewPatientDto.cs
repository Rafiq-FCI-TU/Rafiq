using Microsoft.AspNetCore.Http;

namespace Rafiq.Infrastructure.Services
{
    public class SessionCreateReviewPatientDto
    {
        public IFormFile Media { get; set; } = null!;

        /// <summary>
        /// Optional thumbnail image for the session
        /// </summary>
        /// <example>thumb1.png</example>
    }
}