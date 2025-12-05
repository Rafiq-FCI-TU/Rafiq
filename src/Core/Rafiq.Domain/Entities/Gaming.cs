namespace Rafiq.Domain.Entities
{
    public class Gaming
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int TotalScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<GameCategory> Categories { get; set; } = new List<GameCategory>();

        //Calculating overall progress
        public double OverallProgress => Categories.Any() ? Categories.Average(c => c.Progress) : 0;
    }
}
