namespace Rafiq.Application.Dtos.StepsDtos;
public class PasswordStep3Dto
{
    public string Token { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string ConfirmPassword { get; init; } = null!;
}
