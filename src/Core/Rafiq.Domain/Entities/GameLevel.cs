using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;
public class GameLevel : BaseEntity
{
    public int GameId { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public int LevelPoints { get; set; }
    public int PointsPerQuestion { get; set; }
    public int UnlockThreshold { get; set; }

    // Navigation properties
    public Game Game { get; set; } = null!;
    public List<GameQuestion> Questions { get; set; } = [];
    public List<PatientLevel> PatientLevels { get; set; } = [];
}