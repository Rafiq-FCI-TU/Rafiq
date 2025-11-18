using Rafiq.Application.Dtos.IQDtos;

namespace Rafiq.Application.Interfaces.Services;
public interface IIqService
{
    Task<AssessmentDto> StartAssessmentAsync(int patientId);
    Task<IReadOnlyList<QuestionDto>> GetQuestionsAsync();
    Task SaveAnswersAsync(int patientId, IReadOnlyList<AnswerDto> answers);
    Task<bool> HasAssessmentAsync(int patientId);
    Task<AssessmentProgressDto> GetAssessmentProgressAsync(int patientId);
    Task DeleteAssessmentAsync(int assessmentId);
}
