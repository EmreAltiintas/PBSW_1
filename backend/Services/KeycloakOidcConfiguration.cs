namespace PBSW_1.Services;

/// <summary>
/// Values aligned with OpenID discovery (<c>.well-known/openid-configuration</c>).
/// If <see cref="AuthorizationEndpoint"/> / <see cref="TokenEndpoint"/> are omitted, they are derived from <see cref="Authority"/>.
/// </summary>
public sealed class KeycloakOidcConfiguration
{
    public string Authority { get; init; } = "";
    public string? AuthorizationEndpoint { get; init; }
    public string? TokenEndpoint { get; init; }

    public string ResolveAuthorizationEndpoint()
    {
        if (!string.IsNullOrEmpty(AuthorizationEndpoint))
            return AuthorizationEndpoint;
        return $"{Authority.TrimEnd('/')}/protocol/openid-connect/auth";
    }

    public string ResolveTokenEndpoint()
    {
        if (!string.IsNullOrEmpty(TokenEndpoint))
            return TokenEndpoint;
        return $"{Authority.TrimEnd('/')}/protocol/openid-connect/token";
    }
}
