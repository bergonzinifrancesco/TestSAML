using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestSAML.Api.Controllers.Login.Saml.Callback;

[ApiController]
[Authorize]
[Route("/saml/callback")]
[Tags("SAML")]
public sealed class SamlCallbackController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    public Task<IActionResult> ChallengeSamlAsync()
    {
        var authResult = HttpContext.Features.Get<IAuthenticateResultFeature>()?.AuthenticateResult;
        
        if (authResult?.Properties is null)
            return Task.FromResult<IActionResult>(Unauthorized());

        if (!authResult.Properties.Items.TryGetValue("returnUrl", out var url) || url is null)
            return Task.FromResult<IActionResult>(BadRequest());
        
        return Task.FromResult<IActionResult>(Redirect(url));
    }
}