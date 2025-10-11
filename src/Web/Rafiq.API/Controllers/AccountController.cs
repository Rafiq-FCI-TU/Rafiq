using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Rafiq.API.Controllers;
public class AccountController : BaseController
{
    [HttpPost("register")]
    public ActionResult Register(RegisterRequest model)
    {
        return Ok();
    }

    [HttpPost("login")]
    public ActionResult Login(LoginRequest model)
    {
        return Ok();
    }
}
