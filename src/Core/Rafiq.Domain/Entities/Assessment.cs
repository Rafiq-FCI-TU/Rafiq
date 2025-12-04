using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;
public class Assessment:BaseEntity
{
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CurrentQuestionNumber { get; set; } = 1;
    public AssessmentStatus Status { get; set; } = AssessmentStatus.InProgress;

    // Navigation properties
    public ICollection<Question> Questions { get; set; } = [];
}

  
