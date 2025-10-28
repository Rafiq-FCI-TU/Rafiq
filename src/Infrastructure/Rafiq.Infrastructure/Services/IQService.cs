using Rafiq.Application.Dtos.IQDto;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;
namespace Rafiq.Infrastructure.Services
{
    public class IQService(IUnitOfWork _unitOfWork) : IIQService
    {
        public async Task<AssessmentDto> StartAssessmentAsync(int patientId)
        {
            ///TODO FindSpeAsync(a => a.PatientId == patientId && a.Status == AssessmentStatus.InProgress):this is specification pattern Must be implemented in the repository 
            var assessment = new Assessment
            {
                PatientId = patientId,
                Status = AssessmentStatus.InProgress,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Assessment>().AddAsync(assessment);
            await _unitOfWork.CompleteAsync();

            return new AssessmentDto(
                assessment.Id,
                assessment.PatientId,
                assessment.Status.ToString(),
                assessment.CompletedAt
            );
        }
        public async Task<IReadOnlyList<QuestionDto>> GetQuestionsAsync()
        {
            var questions = await _unitOfWork.Repository<Question>().GetAllAsync();
            return questions.Select(q => new QuestionDto(
                q.Id,
                q.Text,
                q.Category.ToString(),
                q.AnswerType,
                q.Answers.Select(a => a.FrequencyAnswer.HasValue
                    ? a.FrequencyAnswer.Value.ToString()
                    : a.DifficultyAnswer.Value.ToString()).ToList()
            )).ToList();
        }

        public async Task SaveAnswersAsync(int assessmentId, IReadOnlyList<AnswerDto> answers)
        {
            foreach (var a in answers)
            {
                Frequency? freqAnswer = null;
                DifficultyLevel? diffAnswer = null;

                if (Enum.TryParse<Frequency>(a.SelectedOption, out var freq))
                {
                    freqAnswer = freq;
                }
                
                else if (Enum.TryParse<DifficultyLevel>(a.SelectedOption, out var diff))
                {
                    diffAnswer = diff;
                }

                var answer = new Answer
                {
                    AssessmentId = assessmentId,
                    QuestionId = a.QuestionId,
                    FrequencyAnswer = freqAnswer,
                    DifficultyAnswer = diffAnswer
                };

                await _unitOfWork.Repository<Answer>().AddAsync(answer);
            }

            await _unitOfWork.CompleteAsync();
        }
        
        public async Task<bool> HasAssessmentAsync(int patientId)
        {
            var assessment=await _unitOfWork.Repository<Assessment>().GetEntityAsync(patientId);
            return assessment is not null;  
        }


        public async Task<AssessmentProgressDto> GetAssessmentProgressAsync(int patientId)
        {
            var assessment = await _unitOfWork.Repository<Assessment>().GetEntityAsync(patientId);
            return new AssessmentProgressDto(assessment.Id,assessment.PatientId,assessment.Status,assessment.CreatedAt);
        }

        public async Task DeleteAssessmentAsync(int assessmentId)
        {
            var assessment = await _unitOfWork.Repository<Assessment>().GetEntityAsync(assessmentId);
            _unitOfWork.Repository<Assessment>().Delete(assessment);
            await _unitOfWork.CompleteAsync();
        }
    }
}
