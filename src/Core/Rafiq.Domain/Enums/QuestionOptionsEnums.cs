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
    [AnswerEnum(typeof(Frequency))]
    Frequency = 1,

    [AnswerEnum(typeof(DifficultyLevel))]
    DifficultyLevel = 2
}

[AttributeUsage(AttributeTargets.Field)]
public class AnswerEnumAttribute(Type enumType) : Attribute
{
    public Type EnumType { get; } = enumType;
}