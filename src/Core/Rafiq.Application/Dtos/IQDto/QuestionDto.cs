namespace Rafiq.Application.Dtos.IQDto
{
    public record QuestionDto(
        int Id,
        string Text,
        string Category,       
        string AnswerType,
        IReadOnlyList<string> Options
    );
}
