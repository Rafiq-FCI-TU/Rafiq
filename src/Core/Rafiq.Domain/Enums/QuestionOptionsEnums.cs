using System.ComponentModel.DataAnnotations;

namespace Rafiq.Domain.Enums
{
    public enum Frequency
    {
        [Display(Name = "Not Yet")]
        NotYet,
        Sometimes,
        Often,
        Always
    }

    public enum DifficultyLevel
    {
        Low,
        Middle,
        High
    }

}
