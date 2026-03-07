using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;
public class Question : BaseEntity
{
    public string Text { get; set; } = string.Empty; // "Can the child form simple sentences?"
    public int Order { get; set; }
    public double score { get; set; }
    public AnswerType AnswerType { get; set; } // "Frequency", "DifficultyLevel"

    public int CategoryId { get; set; }
}