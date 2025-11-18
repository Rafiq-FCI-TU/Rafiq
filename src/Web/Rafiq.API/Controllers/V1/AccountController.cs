//using Microsoft.AspNetCore.Mvc;
//using Rafiq.API.Controllers;
//using Rafiq.Application.Dtos;
//using Rafiq.Infrastructure.Services.Auth;

//[ApiVersion("1.0")] 
//public class AccountController : BaseController
//{
//    private readonly IAuthService _authService;

//    public AccountController(IAuthService authService) => _authService = authService;

//    [HttpPost("register")]
//    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto model)
//    {
//        if (!ModelState.IsValid)
//            return BadRequest(ModelState);

//        var result = await _authService.RegisterAsync(model);

//        if (!result.IsAuthenticated)
//            return BadRequest(result.Message);

//        if (!string.IsNullOrEmpty(result.RefreshToken))
//            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

//        return Ok(result);
//    }

//    [HttpPost("login")]
//    public async Task<IActionResult> Login(LoginRequestDto model)
//    {
//        var result = await _authService.LoginAsync(model);

//        if (!result.IsAuthenticated)
//            return BadRequest(result.Message);

//        if (!string.IsNullOrEmpty(result.RefreshToken))
//            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

//        return Ok(result);
//    }

//    [HttpGet("refreshToken")]
//    public async Task<IActionResult> RefreshToken()
//    {
//        var refreshToken = Request.Cookies["refreshToken"];

//        if (string.IsNullOrEmpty(refreshToken))
//            return BadRequest("Refresh token is required!");

//        var result = await _authService.RefreshTokenAsync(refreshToken);

//        if (!result.IsAuthenticated)
//            return BadRequest(result.Message);

//        if (!string.IsNullOrEmpty(result.RefreshToken))
//            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

//        return Ok(result);
//    }

//    [HttpPost("revokeToken")]
//    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto model)
//    {
//        var token = model.Token ?? Request.Cookies["refreshToken"];

//        if (string.IsNullOrEmpty(token))
//            return BadRequest("Token is required!");

//        var result = await _authService.RevokeTokenAsync(token);

//        if (!result)
//            return BadRequest("Token is invalid!");

//        return Ok();
//    }

//    private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
//    {
//        var cookieOptions = new CookieOptions
//        {
//            HttpOnly = true,
//            Expires = expires.ToLocalTime(),
//            Secure = true,
//            IsEssential = true,
//            SameSite = SameSiteMode.None
//        };

//        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
//    }
//}