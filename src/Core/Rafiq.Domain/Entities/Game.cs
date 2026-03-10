using Rafiq.Domain.Common;
using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities;

public class Game : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
   
    // Navigation properties
    public GameCategory Category { get; set; } = null!;
    public List<GameLevel> Levels { get; set; } = new List<GameLevel>();
}
