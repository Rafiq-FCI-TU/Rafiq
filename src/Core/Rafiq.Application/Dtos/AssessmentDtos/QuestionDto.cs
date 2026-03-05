using Rafiq.Domain.Enums;

namespace Rafiq.Application.Dtos.AssessmentDtos;
public class QuestionDto
{
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public AnswerType AnswerType { get; set; }
    public List<AnswerOptionDto> Options { get; set; } = [];
}