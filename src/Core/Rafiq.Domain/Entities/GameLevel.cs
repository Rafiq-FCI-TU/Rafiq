using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;

public class GameLevel : BaseEntity
{
    public int GameId { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public int RequiredScore { get; set; }
    public int MaxScore { get; set; }

    // Navigation properties
    public Game Game { get; set; } = null!;
    public List<PatientLevel> PatientLevels { get; set; } = [];
}
