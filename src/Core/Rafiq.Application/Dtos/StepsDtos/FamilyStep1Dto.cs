using Swashbuckle.AspNetCore.Annotations;

namespace Rafiq.Application.Dtos.StepsDtos
{
    /// <summary>
    /// DTO for Step 1 of family registration, containing basic family information.
    /// </summary>
    /// <remarks>
    /// This DTO is used to submit the family head's personal information, contact details,
    /// relationship type, and emergency contact information. It is the first step in the
    /// multi-step registration process.
    /// </remarks>
    public class FamilyStep1Dto
    {
        /// <summary>
        /// First name of the family head.
        /// </summary>
        /// <example>John</example>
        public string FirstName { get; init; } = null!;

        /// <summary>
        /// Last name of the family head.
        /// </summary>
        /// <example>Doe</example>
        public string LastName { get; init; } = null!;

        /// <summary>
        /// Email address of the family head.
        /// </summary>
        /// <example>john.doe@example.com</example>
        public string Email { get; init; } = null!;

        /// <summary>
        /// Phone number of the family head.
        /// </summary>
        /// <example>+201234567890</example>
        public string Phone { get; init; } = null!;

        /// <summary>
        /// Relationship of the family head. Allowed values: Father, Mother, Other.
        /// </summary>
        /// <example>Father</example>
        [SwaggerSchema(Description = "Allowed values: Father, Mother, Other")]
        public string Relationship { get; init; } = null!;

        /// <summary>
        /// Address of the family head.
        /// </summary>
        /// <example>123 Main Street, Cairo, Egypt</example>
        public string Address { get; init; } = null!;

        /// <summary>
        /// Name of the emergency contact person.
        /// </summary>
        /// <example>Jane Doe</example>
        public string EmergencyContactName { get; init; } = null!;

        /// <summary>
        /// Phone number of the emergency contact person.
        /// </summary>
        /// <example>+201112223334</example>
        public string EmergencyContactPhone { get; init; } = null!;
    }
}
