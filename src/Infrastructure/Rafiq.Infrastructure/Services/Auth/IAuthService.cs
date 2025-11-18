using Rafiq.Application.Dtos;

namespace Rafiq.Infrastructure.Services.Auth;

public interface IAuthService
{
    Task<ResponseDto> RegisterAsync(RegisterRequestDto model);
    Task<ResponseDto> LoginAsync(LoginRequestDto model);
    Task<ResponseDto> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
}
