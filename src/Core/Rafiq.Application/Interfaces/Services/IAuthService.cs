using Rafiq.Application.Dtos;
using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;
public interface IAuthService
{
    Task<Result<ResponseDto>> LoginAsync(LoginRequestDto model);
    Task<Result<ResponseDto>> RefreshTokenAsync(string? token);
    Task<Result<bool>> RevokeTokenAsync(string? token);
}
