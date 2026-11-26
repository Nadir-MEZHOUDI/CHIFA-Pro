using System.Threading.RateLimiting;
using CHIFA.Gateway;
using CHIFA.Gateway.Features.Account;
using CHIFA.Gateway.Features.Admin;
using CHIFA.Gateway.Features.Agents;
using CHIFA.Gateway.Features.Auth;
using CHIFA.Gateway.Features.Link;
using CHIFA.Gateway.Features.Officine;
using CHIFA.Gateway.Features.Push;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<GatewayOptions>().BindConfiguration(GatewayOptions.Section).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddOptions<JwtOptions>().BindConfiguration(JwtOptions.Section).ValidateDataAnnotations().ValidateOnStart();

builder.Services.AddDbContextFactory<GatewayDbContext>(o => o
    // Retries cover PostgreSQL still starting when the gateway boots (e.g. both restarted with the VPS).
    .UseNpgsql(builder.Configuration.GetConnectionString("Gateway"), npgsql => npgsql.EnableRetryOnFailure())
    .UseSnakeCaseNamingConvention());

var jwt = builder.Configuration.GetSection(JwtOptions.Section).Get<JwtOptions>() ?? new JwtOptions();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.MapInboundClaims = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = AuthEndpoints.SigningKey(jwt),
            NameClaimType = "name",
            ClockSkew = TimeSpan.FromMinutes(1)
        };
        o.Events = new JwtBearerEvents { OnTokenValidated = AuthEndpoints.RejectRevokedDeviceAsync };
    })
    .AddScheme<AuthenticationSchemeOptions, AgentKeyAuthenticationHandler>(AgentKeyAuthenticationHandler.SchemeName, null);
builder.Services.AddAuthorization();

builder.Services.AddSignalR(o =>
{
    o.KeepAliveInterval = TimeSpan.FromSeconds(15);
    o.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
    // Agent answers (pages of rejets, psychotropes...) are larger than the 32 KB default.
    o.MaximumReceiveMessageSize = 4 * 1024 * 1024;
});

var authRequestsPerMinute = builder.Configuration.GetSection(GatewayOptions.Section).Get<GatewayOptions>()?.AuthRequestsPerMinute ?? 10;
builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.AddPolicy(AuthEndpoints.LoginRateLimit, http => RateLimitPartition.GetFixedWindowLimiter(
        http.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = authRequestsPerMinute, Window = TimeSpan.FromMinutes(1) }));
});

builder.Services.Configure<ForwardedHeadersOptions>(o =>
{
    o.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Only Nginx on the same host can reach the container (port bound to 127.0.0.1), so trust it.
    o.KnownIPNetworks.Clear();
    o.KnownProxies.Clear();
});

builder.Services.AddProblemDetails();
builder.Services.AddSingleton<AgentRegistry>();
builder.Services.AddSingleton<AgentInvoker>();
builder.Services.AddHttpClient(nameof(PushSender));
builder.Services.AddSingleton<PushSender>();
builder.Services.AddHostedService<AlertPoller>();
builder.Services.AddSingleton<IPasswordHasher<MobileUser>, PasswordHasher<MobileUser>>();

var app = builder.Build();

await using (var db = await app.Services.GetRequiredService<IDbContextFactory<GatewayDbContext>>().CreateDbContextAsync())
{
    if (db.Database.IsNpgsql())
        await db.Database.MigrateAsync();
    else
        await db.Database.EnsureCreatedAsync();
}

app.UseForwardedHeaders();
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

// Cloudflare turns the phone app's "no-cache" (index.html, service worker, unversioned scripts) into a 4-hour
// browser cache, so phones would keep the old version; it leaves "private" responses alone.
app.Use((context, next) =>
{
    var response = context.Response;
    response.OnStarting(() =>
    {
        var cacheControl = response.Headers.CacheControl.ToString();
        if (cacheControl == "no-cache" || (cacheControl.Length == 0 && response.ContentType?.StartsWith("text/html") == true))
            response.Headers.CacheControl = "private, no-cache";
        return Task.CompletedTask;
    });
    return next();
});

app.MapGet("/health", () => Results.Ok("ok")).AllowAnonymous();
app.MapHub<AgentConnectionHub>(AgentHub.Route);
app.MapAuthEndpoints();
app.MapLinkEndpoints();
app.MapAdminEndpoints();
app.MapOfficineEndpoints();
app.MapPushEndpoints();
app.MapAccountEndpoints();

// Phone app (CHIFA.Mobile). Unknown API paths stay 404 instead of returning the app's index.html.
app.MapStaticAssets();
app.MapFallbackToFile("index.html");
foreach (var prefix in new[] { "api", "admin", "hubs" })
    app.MapFallback($"{prefix}/{{**rest}}", () => Results.NotFound());

app.Run();

public partial class Program;
