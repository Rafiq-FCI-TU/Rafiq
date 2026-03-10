using System.ComponentModel.DataAnnotations;

namespace Rafiq.Domain.Enums;
public enum AnswerType
{
    [Display(Name = "Not Yet")]
    NotYet = 0,
    [Display(Name = "With Help")]
    WithHelp = 1,
    [Display(Name = "Yes, confidently")]
    YesConfidently = 2
}