namespace Rafiq.Domain.Entities
{
    public class GameCategory
    {
        public int Id { get; set; }
        public int GamingId { get; set; }
        public int InitialScore { get; set; } //from assessment
        public string Name { get; set; }
        public List<Game> Games { get; set; } = new List<Game>();

        // Progress based on games
        public double Progress => Games.Any() ? Games.Average(g => g.Progress) : 0;
    }
}
