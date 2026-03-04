namespace Rafiq.Application.Dtos.SessionDtos
{
    public class SessionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public int Score { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;

        public string SpecialistProfileId { get; set; } = string.Empty;
        public string SpecialistName { get; set; } = string.Empty; 
    }
}
namespace Rafiq.Application.Dtos.SessionDtos
{
    public class Sessionfor_Specialist_patientDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public int Sequence { get; set; }
        public int Score { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;

        public string SpecialistProfileId { get; set; } = string.Empty;
        public int PatientId { get; set; }

    }
}