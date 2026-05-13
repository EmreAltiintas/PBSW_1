using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PBSW_1.Models;
using PBSW_1.Services;

namespace PBSW_1.Controllers;

public class HomeController(IHttpClientFactory httpClientFactory, IConfiguration configuration) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>OAuth redirect_uri — exchanges authorization code for tokens (PKCE).</summary>
    [HttpGet]
    public async Task<IActionResult> Callback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        [FromQuery] string? error,
        [FromQuery] string? error_description)
    {
        if (!string.IsNullOrEmpty(error))
            return Content($"OAuth error: {error}. {error_description}");

        if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            return BadRequest("Missing code or state.");

        var storedState = HttpContext.Session.GetString(AuthController.SessionStateKey);
        var codeVerifier = HttpContext.Session.GetString(AuthController.SessionCodeVerifierKey);
        var redirectUriUsed = HttpContext.Session.GetString(AuthController.SessionRedirectUriKey);
        var returnUrl = HttpContext.Session.GetString(AuthController.SessionReturnUrlKey) ?? Url.Content("~/");

        HttpContext.Session.Remove(AuthController.SessionStateKey);
        HttpContext.Session.Remove(AuthController.SessionCodeVerifierKey);
        HttpContext.Session.Remove(AuthController.SessionRedirectUriKey);
        HttpContext.Session.Remove(AuthController.SessionReturnUrlKey);

        if (string.IsNullOrEmpty(storedState) || storedState != state
            || string.IsNullOrEmpty(codeVerifier) || string.IsNullOrEmpty(redirectUriUsed))
            return BadRequest("Invalid or expired login session.");

        var cfg = configuration.GetSection("Keycloak").Get<KeycloakOidcConfiguration>()
            ?? throw new InvalidOperationException("Missing Keycloak configuration.");
        var tokenEndpoint = cfg.ResolveTokenEndpoint();
        var clientId = configuration["Keycloak:ClientId"]
            ?? throw new InvalidOperationException("Keycloak:ClientId");
        var clientSecret = configuration["Keycloak:ClientSecret"];

        var form = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = redirectUriUsed,
            ["client_id"] = clientId,
            ["code_verifier"] = codeVerifier,
        };
        if (!string.IsNullOrEmpty(clientSecret))
            form["client_secret"] = clientSecret;

        using var content = new FormUrlEncodedContent(form);
        var http = httpClientFactory.CreateClient();
        var response = await http.PostAsync(tokenEndpoint, content);
        var body = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            return Content($"Token exchange failed: {(int)response.StatusCode}\n{body}");

        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;
        if (!root.TryGetProperty("id_token", out var idTok))
            return Content("No id_token in token response.");

        var idToken = idTok.GetString();
        if (string.IsNullOrEmpty(idToken))
            return Content("Empty id_token.");

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(idToken);
        var identity = new ClaimsIdentity(
            jwt.Claims,
            CookieAuthenticationDefaults.AuthenticationScheme,
            nameType: "preferred_username",
            roleType: ClaimTypes.Role);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));

        if (!Url.IsLocalUrl(returnUrl))
            returnUrl = Url.Content("~/");

        return LocalRedirect(returnUrl);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
