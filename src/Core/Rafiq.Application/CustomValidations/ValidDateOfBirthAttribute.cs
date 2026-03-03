using System.ComponentModel.DataAnnotations;

namespace Rafiq.Application.CustomValidations;
public class ValidDateOfBirthAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateTime dob)
            return new ValidationResult("Date of birth is required.");

        if (dob > DateTime.UtcNow)
            return new ValidationResult("Date of birth cannot be in the future.");

        var age = DateTime.UtcNow.Year - dob.Year;
        if (age < 0 || age > 120)
            return new ValidationResult("Invalid patient age.");

        return ValidationResult.Success;
    }
}