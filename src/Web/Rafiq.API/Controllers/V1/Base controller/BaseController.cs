using Microsoft.AspNetCore.Mvc;

namespace Rafiq.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseController : ControllerBase { }
