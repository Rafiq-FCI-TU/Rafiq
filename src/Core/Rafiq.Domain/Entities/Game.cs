using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;

public class Game : BaseEntity
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public string FeedbackCorrect { get; set; } = string.Empty;
    public string FeedbackWrong { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public List<GameLevel> Levels { get; set; } = new List<GameLevel>();
}
