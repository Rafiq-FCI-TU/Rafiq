using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;
public class Assessment : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int TotalScore { get; set; }
    public int QuestionsNumber { get; set; } 
    public string CurrentCategory { get; set; } = string.Empty;
    public AssessmentStatus Status { get; set; } = AssessmentStatus.InProgress;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public List<AssessmentCategory> Categories { get; set; } = [];
} 

public class AssessmentDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public AssessmentStatus Status { get; set; } = AssessmentStatus.InProgress;

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public List<AssessmentCategory> Categories { get; set; } = [];
}


