using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;
public class AssessmentAnswer : BaseEntity
{
    public int AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;

    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    // enum value
    public int AnswerValue { get; set; }
}