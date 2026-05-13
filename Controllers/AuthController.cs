using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace PBSW_1.Controllers;

public class AuthController : Controller
{
    /// <summary>
    /// Sends the user to Keycloak (<c>config.authorization_endpoint</c>) via the OpenID Connect middleware.
    /// </summary>
    [HttpGet("/login")]
    public IActionResult Login([FromQuery] string? returnUrl)
    {
        var redirectUri = string.IsNullOrEmpty(returnUrl)
            ? Url.Content("~/")
            : Url.IsLocalUrl(returnUrl)
                ? returnUrl
                : Url.Content("~/");

        return Challenge(
            new AuthenticationProperties { RedirectUri = redirectUri },
            OpenIdConnectDefaults.AuthenticationScheme);
    }
}
