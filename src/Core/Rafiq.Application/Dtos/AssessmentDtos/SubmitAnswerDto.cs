namespace Rafiq.Application.Dtos.AssessmentDtos;
public class SubmitAnswerDto
{
    public int AssessmentId { get; set; }
    public int QuestionId { get; set; }
    public int AnswerValue { get; set; }
}