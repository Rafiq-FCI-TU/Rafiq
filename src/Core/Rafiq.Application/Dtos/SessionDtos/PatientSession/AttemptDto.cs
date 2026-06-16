/// <summary>
/// Data Transfer Object representing a patient's attempt for a session.
/// Used to return attempt details to the client.
/// </summary>
public class AttemptDto
{
    /// <summary>
    /// Unique identifier of the attempt.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identifier of the session associated with this attempt.
    /// </summary>
    public int SessionId { get; set; }

    /// <summary>
    /// Title of the session that the patient attempted.
    /// </summary>
    public string SessionTitle { get; set; } = string.Empty;

    /// <summary>
    /// URL of the session video related to this attempt.
    /// </summary>
    public string VideoUrl { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the attempt (e.g., PendingReview, Approved, Rejected).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Feedback provided by the doctor after reviewing the attempt.
    /// </summary>
    public string? DoctorFeedback { get; set; }

    /// <summary>
    /// Score awarded by the doctor if the attempt is approved.
    /// </summary>
    public int? ScoreAwarded { get; set; }

    /// <summary>
    /// Date and time when the attempt was submitted.
    /// </summary>
    public DateTime SubmittedAt { get; set; }
}
