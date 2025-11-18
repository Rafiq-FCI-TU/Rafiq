namespace Rafiq.Application.Dtos.IQDtos;
public class AssessmentDto
{
    public int Id { get; init; }
    public int PatientId { get; init; }
    public string Status { get; init; } = null!;
    public IReadOnlyList<QuestionDto> Questions { get; init; } = [];
    public DateTime? CompletedAt { get; init; }
}
