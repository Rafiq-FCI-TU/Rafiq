using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;

public class Game : BaseEntity
{
    public int CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int MaxPoints { get; set; }
    public string Duration { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public GameStatus Status { get; set; } = GameStatus.NotStarted;

    // Navigation properties
    public GameCategory Category { get; set; } = null!;
    public List<GameLevel> Levels { get; set; } = new List<GameLevel>();

    public double Progress
    {
        get
        {
            if (!Levels.Any()) return 0;
            var totalAchieved = Levels
                .SelectMany(l => l.ProgressRecords)
                .Sum(pr => pr.EarnedPoints);
            return (double)totalAchieved / MaxPoints * 100;
        }
    }
}
