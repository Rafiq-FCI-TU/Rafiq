using Rafiq.Application.Interfaces.Services;

namespace Rafiq.Application.Dtos.AssessmentDtos;
public class AssessmentCategoryDto
{
    public int AssessmentCategoryId { get; set; }
    public string AssessmentCategoryName { get; set; } = string.Empty;
    public string AssessmentCategoryDescription { get; set; } = string.Empty;
    public int AssessmentCurrentQuestion { get; set; }
    public int AssessmentTotalQuestions { get; set; }
    public double Score { get; set; }

    public QuestionDto? Question { get; set; }
}