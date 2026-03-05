using Rafiq.Application.Dtos.AssessmentDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;
public interface IAssessmentService
{
    Task<Result<AssessmentResponseDto>> CreateAssessmentForPatientAsync(int patientId);

    Task<Result<AssessmentStatusDto>> GetAssessmentStatusAsync(int assessmentId);

    Task<Result<AssessmentStatusDto>> SubmitAnswerAsync(SubmitAnswerDto dto);
}