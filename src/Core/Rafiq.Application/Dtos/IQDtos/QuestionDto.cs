namespace Rafiq.Application.Dtos.IQDtos;
public class QuestionDto
{
    public int Id { get; init; }
    public string Text { get; init; } = null!;
    public string Category { get; init; } = null!;
    public string AnswerType { get; init; } = null!;
    public IReadOnlyList<string> Options { get; init; } = [];
}
