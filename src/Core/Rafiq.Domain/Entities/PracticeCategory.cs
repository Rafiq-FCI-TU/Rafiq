using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;
public class PracticeCategory: BaseEntity
{
    public double Score { get; set; }
    public double QuestionsEarnedPoints { get; set; }
    public double QuestionsMaxPoints { get; set; } // dynamic: Current question * 10 (question points - static)
    public int QuestionsNumber { get; set; } = 10;
    public int CurrentQuestion { get; set; } = 1;
    public string CategoryType { get; set; } = string.Empty;

    public int AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}


