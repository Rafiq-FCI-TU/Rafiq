using Rafiq.Domain.Enums;

namespace Rafiq.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int MaxPoints { get; set; }
        public string Duration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public GameStatus Status { get; set; } = GameStatus.NotStarted;
        public List<GameProgress> ProgressRecords { get; set; } = new List<GameProgress>();

        public double Progress
        {
            get
            {
                if (!ProgressRecords.Any()) return 0;
                var totalAchieved = ProgressRecords.Sum(pr => pr.AchievedPoints);
                return (double)totalAchieved / MaxPoints * 100;
            }
        }
    }
}
