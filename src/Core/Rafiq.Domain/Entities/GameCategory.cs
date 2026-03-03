using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities
{
    public class GameCategory : BaseEntity
    {
        public int InitialScore { get; set; } //from assessment
        public string Name { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();

        // Progress based on games
        public double Progress => Games.Any() ? Games.Average(g => g.Progress) : 0;
    }
}
