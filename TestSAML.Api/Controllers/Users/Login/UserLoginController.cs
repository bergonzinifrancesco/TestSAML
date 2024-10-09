using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestSAML.Api.Controllers.Users.Login;

[ApiController]
[Authorize]
[Route("/user/login")]
[Tags("Login")]
public sealed class UserLoginController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(302)]
    public async Task<IActionResult> UserLogin()
    {
        return Ok();
    }
}