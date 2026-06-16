using Rafiq.Domain.Common;
using Rafiq.Domain.Entities;

namespace Rafiq.Domain.Entities;
public class Answer:BaseEntity
{
    public int QuestionId { get; set; }
    public Question Question { get; set; } = null!;
    public string SelectedOption { get; set; } = string.Empty;
}