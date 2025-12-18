namespace Rafiq.Application.Dtos.StepsDtos
{
    /// <summary>
    /// DTO for Step 2 of family registration, containing patient details.
    /// </summary>
    /// <remarks>
    /// This DTO is used to submit the patient's personal information, including first name,
    /// last name, date of birth, gender, and the registration session token obtained from Step 1.
    /// </remarks>
    public class PatientStep2Dto
    {
        /// <summary>
        /// Registration session token obtained from Step 1.
        /// </summary>
        /// <example>pending_registration:abc123xyz</example>
        public string Token { get; init; } = null!;

        /// <summary>
        /// First name of the patient.
        /// </summary>
        /// <example>Alex</example>
        public string FirstName { get; init; } = null!;

        /// <summary>
        /// Last name of the patient.
        /// </summary>
        /// <example>Doe</example>
        public string LastName { get; init; } = null!;

        /// <summary>
        /// Date of birth of the patient.
        /// </summary>
        /// <example>2015-08-12</example>
        public DateTime DateOfBirth { get; init; }

        /// <summary>
        /// Gender of the patient. Possible values: Male, Female.
        /// </summary>
        /// <example>Male</example>
        public string Gender { get; init; } = null!;
    }
}
