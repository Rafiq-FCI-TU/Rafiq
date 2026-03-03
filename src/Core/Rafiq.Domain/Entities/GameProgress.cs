using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities
{
    public class GameProgress : BaseEntity // A class to track progress in each game
    {
        public int GameId { get; set; }
        public int PatientId { get; set; }
        public int AchievedPoints { get; set; }

        public int TimePlayed { get; set; }// Time spent (in minutes)
        public bool Completed { get; set; } // Is the game over?
        public DateTime PlayedAt { get; set; } = DateTime.Now; // Playdate

        // Navigation
        public Game Game { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
    }
}
