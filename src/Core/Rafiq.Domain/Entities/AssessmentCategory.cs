using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;
public class AssessmentCategory: BaseEntity
{
    public double Score { get; set; }
    public int QuestionsNumber { get; set; } = 10;
    public int CurrentQuestion { get; set; } = 1;

    public int AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}