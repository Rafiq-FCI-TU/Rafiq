using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;
public class PatientLevel : BaseEntity
{
    public int PatientId { get; set; }
    public int GameLevelId { get; set; }
    public int EarnedPoints { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public List<GameLevel> GameLevel { get; set; } = null!;
}
