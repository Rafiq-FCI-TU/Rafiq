using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rafiq.API.Extensions;
using Rafiq.Application.Dtos;
using Rafiq.Application.Dtos.AuthDtos;
using Rafiq.Application.Interfaces.Services;

namespace Rafiq.API.Controllers;
public class AuthController(IAuthService authService, IForgotPasswordService forgotPasswordService) : BaseController
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
        return (await authService.LoginAsync(model)).ToActionResult();
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto model)
    {
        return (await authService.RefreshTokenAsync(model.RefreshToken)).ToActionResult();
    }

    [HttpPost("revoke-token")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestDto model)
    {
        return (await authService.RevokeTokenAsync(model.RefreshToken)).ToActionResult();
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto model)
    {
        return (await forgotPasswordService.ForgotPasswordAsync(model)).ToActionResult();
    }

    [HttpPost("verify-otp")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto model)
    {
        return (await forgotPasswordService.VerifyOtpAsync(model)).ToActionResult();
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto model)
    {
        return (await forgotPasswordService.ResetPasswordAsync(model)).ToActionResult();
    }
}
