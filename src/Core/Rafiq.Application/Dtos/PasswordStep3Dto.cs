namespace Rafiq.Application.Dtos;

public record PasswordStep3Dto(
    string Token,
    string Password,
    string ConfirmPassword
);
