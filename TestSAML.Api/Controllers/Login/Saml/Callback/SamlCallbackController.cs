using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestSAML.Api.Controllers.Login.Saml.Callback;

[ApiController]
[AllowAnonymous]
[Route("/saml/callback")]
[Tags("SAML")]
public sealed class SamlCallbackController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ChallengeSamlAsync(CancellationToken cancellationToken)
    {
        ;
        return Ok();
    }
}