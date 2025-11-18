using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; set; } = string.Empty; // "Can the child form simple sentences?"
        public int Order { get; set; }
        public AssessmentCategoryType Category { get; set; }
        public string AnswerType { get; set; } = string.Empty; // "Frequency", "DifficultyLevel"
        public ICollection<Answer> Answers { get; set; } = new List<Answer>(); // Family's responses
    }
}