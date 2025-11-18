using Rafiq.Application.Dtos.IQDtos;
using Rafiq.Application.Interfaces.Repositories;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Enums;

namespace Rafiq.Infrastructure.Services;
public class IqService(IUnitOfWork unitOfWork) : IIqService
{
    public async Task<AssessmentDto> StartAssessmentAsync(int patientId)
    {
        ///TODO FindSpeAsync(a => a.PatientId == patientId && a.Status == AssessmentStatus.InProgress):this is specification pattern Must be implemented in the repository 
        var assessment = new Assessment
        {
            PatientId = patientId,
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.Repository<Assessment>().AddAsync(assessment);
        await unitOfWork.CompleteAsync();

        var questions = await GetQuestionsAsync();

        return new AssessmentDto
        {
            Id = assessment.Id,
            PatientId = assessment.PatientId,
            Status = assessment.Status.ToString(),
            Questions = questions
        };
    }

    public async Task<IReadOnlyList<QuestionDto>> GetQuestionsAsync()
    {
        var questionsResponse = new List<QuestionDto>();

        var questions = await unitOfWork.Repository<Question>().GetAllAsync();

        foreach (var question in questions)
        {
            var options = new List<string>();

            switch (question.AnswerType)
            {
                case AnswerType.Frequency:
                    options = Enum.GetNames(typeof(Frequency)).ToList();
                    break;
                case AnswerType.DifficultyLevel:
                    options = Enum.GetNames(typeof(DifficultyLevel)).ToList();
                    break;
                default:
                    break;
            }

            questionsResponse.Add(new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                Category = question.Category.ToString(),
                AnswerType = question.AnswerType.ToString(),
                Options = options
            });
        }

        return questionsResponse;
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
                QuestionId = a.QuestionId,
                //FrequencyAnswer = freqAnswer,
                //DifficultyAnswer = diffAnswer
            };

            await unitOfWork.Repository<Answer>().AddAsync(answer);
        }

        await unitOfWork.CompleteAsync();
    }

    public async Task<bool> HasAssessmentAsync(int patientId)
    {
        var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(patientId);
        return assessment is not null;
    }


    public async Task<AssessmentProgressDto> GetAssessmentProgressAsync(int patientId)
    {
        var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(patientId);
        if (assessment != null)
        {
            return new AssessmentProgressDto
            {
                AssessmentId = assessment.Id,
                PatientId = assessment.PatientId,
                Status = assessment.Status,
                CreatedAt = assessment.CreatedAt
            };
        }

        return new AssessmentProgressDto();
    }

    public async Task DeleteAssessmentAsync(int assessmentId)
    {
        var assessment = await unitOfWork.Repository<Assessment>().GetEntityAsync(assessmentId);
        unitOfWork.Repository<Assessment>().Delete(assessment);
        await unitOfWork.CompleteAsync();
    }
}
