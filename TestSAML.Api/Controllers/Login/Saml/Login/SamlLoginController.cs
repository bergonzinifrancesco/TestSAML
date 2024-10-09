using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sustainsys.Saml2.AspNetCore2;

namespace TestSAML.Api.Controllers.Login.Saml.Login;

[ApiController]
[AllowAnonymous]
[Route("/saml/login")]
[Tags("SAML")]
public sealed class SamlLoginController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ChallengeSamlAsync(
        [FromQuery] string returnUrl,
        CancellationToken cancellationToken
    )
    {
        var props = new AuthenticationProperties
        {
            RedirectUri = "/saml/callback",
            Items = { { "returnUrl", returnUrl } }
        };

        return Challenge(props, Saml2Defaults.Scheme);
    }
}