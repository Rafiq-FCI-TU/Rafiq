using System.ComponentModel.DataAnnotations;

namespace Rafiq.Domain.Enums;
public enum Frequency
{
    [Display(Name = "Not Yet")]
    NotYet = 1,

    Sometimes = 2,
    Often = 3,
    Always = 4
}

// public enum DifficultyLevel
// {
//     Low = 1,
//     Middle = 2,
//     High = 3
// }

public enum AnswerType
{
    Frequency = 1,
    DifficultyLevel = 2
}