using Microsoft.AspNetCore.Http;

/// <summary>
/// Data Transfer Object used by a patient to submit a video attempt for a session.
/// </summary>
public class SubmitAttemptDto
{
    /// <summary>
    /// Identifier of the session that the patient is submitting an attempt for.
    /// </summary>
    public int SessionId { get; set; }

    /// <summary>
    /// Identifier of the patient who is submitting the attempt.
    /// </summary>
    public int PatientId { get; set; }

    /// <summary>
    /// Video file uploaded by the patient as proof of completing the session exercise.
    /// </summary>
    public IFormFile Video { get; set; } = null!;
}
