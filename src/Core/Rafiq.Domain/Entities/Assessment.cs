using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;
public class Assessment:BaseEntity
{
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    // TODO:Delete it   
    public DateTime? CompletedAt { get; set; }

    public AssessmentStatus Status { get; set; } = AssessmentStatus.InProgress;

    // Navigation properties
    public ICollection<Answer> Answers { get; set; } = new List<Answer>();

    /*    // Calculated properties
        public bool IsCompleted => Status == AssessmentStatus.Completed;
        public int TotalScore => Answers.Sum(a => a.GetScore());
        public int MaxPossibleScore => 27; // 9 questions × 3 points max
        public double ProgressPercentage => (double)Answers.Count / 9 * 100;*/
}


