using Rafiq.Domain.Enums;
namespace Rafiq.Application.Dtos.IQDto
{
    public record QuestionDto(
        int Id,
        string Text,
        string Category,       
        string AnswerType,     
        IReadOnlyList<string> Options
    );

    public record AnswerDto(
        int QuestionId,
        string SelectedOption
    );

    public record AssessmentDto(
        int Id,
        int PatientId,
        string Status,      
        DateTime? CompletedAt = null
    );


    public record AssessmentProgressDto(
       int AssessmentId,
       int PatientId,
       AssessmentStatus Status,
        DateTime CreatedAt
    );
}
