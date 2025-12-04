using Rafiq.Domain.Enums;

namespace Rafiq.Application.Dtos.IQDtos;
public class AnswerDto
{
    public int QuestionId { get; init; }
    public string SelectedOption { get; init; } = null!;
    public Frequency? FrequencyAnswer { get; set; }
    public DifficultyLevel? DifficultyAnswer { get; set; }
}
