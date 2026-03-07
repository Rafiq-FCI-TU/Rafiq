using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;

public class GameProgress : BaseEntity
{
    public int PatientId { get; set; }
    public int GameLevelId { get; set; }
    public int EarnedPoints { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.Now;

    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public GameLevel GameLevel { get; set; } = null!;
}
