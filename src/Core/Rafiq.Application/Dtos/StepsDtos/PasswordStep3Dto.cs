namespace Rafiq.Application.Dtos.StepsDtos
{
    /// <summary>
    /// DTO for Step 3 of family registration, containing the password information to complete registration.
    /// </summary>
    /// <remarks>
    /// This DTO is used to finalize the family registration process by setting a password.
    /// It requires the registration token obtained from previous steps.
    /// </remarks>
    public class PasswordStep3Dto
    {
        /// <summary>
        /// Registration session token obtained from Step 1 or Step 2.
        /// </summary>
        /// <example>pending_registration:abc123xyz</example>
        public string Token { get; init; } = null!;

        /// <summary>
        /// Password to create the family account.
        /// </summary>
        /// <example>P@ssw0rd123</example>
        public string Password { get; init; } = null!;

        /// <summary>
        /// Confirmation of the password. Must match <see cref="Password"/>.
        /// </summary>
        /// <example>P@ssw0rd123</example>
        public string ConfirmPassword { get; init; } = null!;
    }
}
