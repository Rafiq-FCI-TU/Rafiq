using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;
public class PatientLevel : BaseEntity
{
    public int PatientId { get; set; }
    public int GameLevelId { get; set; }
    public double EarnedPoints { get; set; }
    public double MaxPoints { get; set; }
    public double Score { get; set; }
    public DateTime PlayedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public GameLevel GameLevel { get; set; } = null!;
}
