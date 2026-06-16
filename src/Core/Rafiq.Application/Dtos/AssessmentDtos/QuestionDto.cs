using Rafiq.Domain.Enums;

namespace Rafiq.Application.Dtos.AssessmentDtos;
public class QuestionDto
{
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;  
    public int Order { get; set; }
    public double MaxPoints { get; set; } = 10;
    public List<AnswerOptionDto> Options { get; set; } = [];
}