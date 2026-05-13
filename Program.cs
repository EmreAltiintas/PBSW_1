using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var keycloakSection = builder.Configuration.GetSection("Keycloak");
var authority = keycloakSection["Authority"] ?? "http://localhost:8080/realms/master";

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie()
.AddOpenIdConnect(options =>
{
    options.Authority = authority;
    options.ClientId = keycloakSection["ClientId"]
        ?? throw new InvalidOperationException("Configure Keycloak:ClientId (see appsettings.json).");
    options.ClientSecret = keycloakSection["ClientSecret"];

    // Redirect URI registered in Keycloak must match this path on the app origin (e.g. https://localhost:7154/callback).
    options.CallbackPath = "/callback";
    options.ResponseType = OpenIdConnectResponseType.Code;
    options.SaveTokens = true;
    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.MapInboundClaims = false;
    options.TokenValidationParameters.NameClaimType = "preferred_username";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
