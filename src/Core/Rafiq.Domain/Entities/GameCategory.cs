using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities;

public class GameCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // Navigation properties
    public List<Game> Games { get; set; } = new List<Game>();
}
