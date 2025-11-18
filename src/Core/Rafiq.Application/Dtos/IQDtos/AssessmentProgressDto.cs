using Rafiq.Domain.Enums;

namespace Rafiq.Application.Dtos.IQDtos;
public class AssessmentProgressDto
{
    public int AssessmentId { get; init; }
    public int PatientId { get; init; }
    public AssessmentStatus Status { get; init; }
    public DateTime CreatedAt { get; init; } = new();
}
