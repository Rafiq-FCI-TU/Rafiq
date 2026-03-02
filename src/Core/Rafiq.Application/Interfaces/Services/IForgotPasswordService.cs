using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Domain.ErrorHandling;

namespace Rafiq.Application.Interfaces.Services;
public interface IForgotPasswordService
{
    Task<Result<bool>> ForgotPasswordAsync(ForgotPasswordRequestDto model);
    Task<Result<bool>> VerifyOtpAsync(VerifyOtpRequestDto model);
    Task<Result<bool>> ResetPasswordAsync(ResetPasswordRequestDto model);
}