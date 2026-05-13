using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using PBSW_1.Services;

namespace PBSW_1.Controllers;

public class AuthController(IConfiguration configuration) : Controller
{
    public const string SessionStateKey = "oidc.state";
    public const string SessionCodeVerifierKey = "oidc.code_verifier";
    public const string SessionReturnUrlKey = "oidc.return_url";
    public const string SessionRedirectUriKey = "oidc.redirect_uri";

    [HttpGet("/login")]
    public IActionResult Login([FromQuery] string? returnUrl)
    {
        var cfg = configuration.GetSection("Keycloak").Get<KeycloakOidcConfiguration>()
            ?? throw new InvalidOperationException("Missing Keycloak configuration.");

        var authorizationEndpoint = cfg.ResolveAuthorizationEndpoint();

        var clientId = configuration["Keycloak:ClientId"]
            ?? throw new InvalidOperationException("Keycloak:ClientId");

        var callback = $"{Request.Scheme}://{Request.Host}/callback";

        var redirectAfter = string.IsNullOrEmpty(returnUrl)
            ? Url.Content("~/")
            : Url.IsLocalUrl(returnUrl)
                ? returnUrl
                : Url.Content("~/");

        var state = CreateRandomUrlSafeString();
        var codeVerifier = CreateRandomUrlSafeString();
        var codeChallenge = CreateCodeChallenge(codeVerifier);

        HttpContext.Session.SetString(SessionStateKey, state);
        HttpContext.Session.SetString(SessionCodeVerifierKey, codeVerifier);
        HttpContext.Session.SetString(SessionReturnUrlKey, redirectAfter);
        HttpContext.Session.SetString(SessionRedirectUriKey, callback);

        var parameters = new Dictionary<string, string?>
        {
            { "client_id", clientId },
            { "scope", "openid email phone address profile" },
            { "response_type", "code" },
            { "redirect_uri", callback },
            { "prompt", "login" },
            { "state", state },
            { "code_challenge_method", "S256" },
            { "code_challenge", codeChallenge }
        };

        var authorizationUri = QueryHelpers.AddQueryString(authorizationEndpoint, parameters);
        return Redirect(authorizationUri);
    }

    private static string CreateRandomUrlSafeString(int byteLength = 32)
    {
        var bytes = new byte[byteLength];
        RandomNumberGenerator.Fill(bytes);

        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static string CreateCodeChallenge(string codeVerifier)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(codeVerifier));

        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}