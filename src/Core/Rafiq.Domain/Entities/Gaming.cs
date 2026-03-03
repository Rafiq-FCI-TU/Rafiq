using Rafiq.Domain.Common;

namespace Rafiq.Domain.Entities
{
    public class Gaming : BaseEntity
    {
        public int PatientId { get; set; }
        public int TotalScore { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
