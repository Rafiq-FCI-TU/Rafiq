using System.ComponentModel.DataAnnotations;

namespace Rafiq.Domain.Enums;
public enum AssessmentCategory
{
    [Display(Name = "Speech & Language")]
    SpeechAndLanguage,

    [Display(Name = "Motor Skills")]
    MotorSkills,

    [Display(Name = "Cognitive Development")]
    CognitiveDevelopment
}
