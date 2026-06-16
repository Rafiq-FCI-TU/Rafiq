/// <summary>
/// Data Transfer Object used by a specialist to review a patient's session attempt.
/// </summary>
public class ReviewAttemptDto
{
    /// <summary>
    /// The unique identifier of the attempt that will be reviewed.
    /// </summary>
    public int AttemptId { get; set; }

    /// <summary>
    /// The review status of the attempt (e.g., Approved or Rejected).
    /// </summary>
    public string Status { get; set; } = null!;

    /// <summary>
    /// Optional feedback provided by the doctor after reviewing the attempt.
    /// </summary>
    public string? DoctorFeedback { get; set; }

    /// <summary>
    /// Score awarded to the patient if the attempt is approved.
    /// </summary>
    public int? ScoreAwarded { get; set; }
}
