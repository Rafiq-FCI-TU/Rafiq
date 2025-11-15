using Rafiq.Domain.Enums;
namespace Rafiq.Application.Dtos.IQDto
{
   public record AssessmentProgressDto(
       int AssessmentId,
       int PatientId,
       AssessmentStatus Status,
        DateTime CreatedAt
    );
}
