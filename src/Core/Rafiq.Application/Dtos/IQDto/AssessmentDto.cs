namespace Rafiq.Application.Dtos.IQDto
{
    public record AssessmentDto(
        int Id,
        int PatientId,
        string Status,      
        IReadOnlyList<QuestionDto> Questions,
        DateTime? CompletedAt = null
    );
}
