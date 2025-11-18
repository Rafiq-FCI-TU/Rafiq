using Rafiq.Application.Dtos;

namespace Rafiq.Application.Interfaces.Services;

public interface IAuthService
{
    Task<ResponseDto> RegisterAsync(RegisterRequestDto model);
    Task<ResponseDto> LoginAsync(LoginRequestDto model);
    Task<ResponseDto> RefreshTokenAsync(string token);
    Task<bool> RevokeTokenAsync(string token);
}
