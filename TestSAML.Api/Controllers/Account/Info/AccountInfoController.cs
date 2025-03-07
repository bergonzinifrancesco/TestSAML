using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestSAML.Api.Controllers.Account.Info;

[ApiController]
[Authorize]
[Route("/account/info")]
[Tags("Login")]
public sealed class AccountInfoController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(302)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> UserLogin()
    {
        return Ok(new
        {
            Claims = User.Claims.Select(x => new
            {
                x.Type,
                x.Value
            }),
            User.Identity?.Name,
            User.Identity?.AuthenticationType
        });
    }
}