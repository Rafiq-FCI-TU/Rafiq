using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;
public class Question : BaseEntity
{
    public string Text { get; set; } = string.Empty; // "Can the child form simple sentences?"
    public int Order { get; set; }
    
    //TODO Delete
    public int AssessmentId { get; set; }
    public Assessment Assessment { get; set; } = null!;
    public AssessmentCategory Category { get; set; }
    // --------------

    public AnswerType AnswerType { get; set; } // "Frequency", "DifficultyLevel"
    public string SelectedAnswer { get; set; } = null!; // Family's responses
}