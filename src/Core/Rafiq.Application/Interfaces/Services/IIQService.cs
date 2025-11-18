using Rafiq.Application.Dtos.IQDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Application.Interfaces.Services
{
    public interface IIQService 
    {
        Task<AssessmentDto> StartAssessmentAsync(int patientId);
        Task<IReadOnlyList<QuestionDto>> GetQuestionsAsync();
        Task SaveAnswersAsync(int patientId, IReadOnlyList<AnswerDto> answers);
        Task<bool> HasAssessmentAsync(int patientId);
        Task<AssessmentProgressDto> GetAssessmentProgressAsync(int patientId);
        Task DeleteAssessmentAsync(int assessmentId);
  
    }

}
