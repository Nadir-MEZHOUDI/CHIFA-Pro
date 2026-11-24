using System.Buffers.Text;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.Extensions.Options;
using WebPushSubscription = Lib.Net.Http.WebPush.PushSubscription;

namespace CHIFA.Gateway.Features.Push;

/// <summary>What the service worker shows (see CHIFA.Mobile service-worker.js).</summary>
public record PushPayload(string Title, string Body, string Url, string Tag);

/// <summary>
/// Sends Web Push notifications. The VAPID key pair is created on first use and kept in the database,
/// so deploying the gateway needs no extra secret and browsers keep their subscriptions across restarts.
/// </summary>
public class PushSender(
    IDbContextFactory<GatewayDbContext> dbFactory,
    IHttpClientFactory httpFactory,
    IOptions<GatewayOptions> options,
    ILogger<PushSender> logger)
{
    private readonly SemaphoreSlim _keyLock = new(1, 1);
    private VapidKey? _key;

    public async Task<string> GetPublicKeyAsync(CancellationToken ct) => (await GetKeyAsync(ct)).PublicKey;

    /// <summary>Sends to every subscription; the ones the push service reports as gone are deleted.</summary>
    public async Task SendAsync(IReadOnlyCollection<Data.PushSubscription> subscriptions, PushPayload payload, CancellationToken ct)
    {
        if (subscriptions.Count == 0)
            return;

        var key = await GetKeyAsync(ct);
        var client = new PushServiceClient(httpFactory.CreateClient(nameof(PushSender)))
        {
            DefaultAuthentication = new VapidAuthentication(key.PublicKey, key.PrivateKey) { Subject = options.Value.PushSubject }
        };
        var message = new PushMessage(JsonSerializer.Serialize(payload, JsonSerializerOptions.Web))
        {
            TimeToLive = (int)TimeSpan.FromDays(1).TotalSeconds,
            Urgency = PushMessageUrgency.Normal
        };

        var gone = new List<Guid>();
        foreach (var subscription in subscriptions)
        {
            try
            {
                await client.RequestPushMessageDeliveryAsync(ToWebPush(subscription), message, ct);
            }
            catch (PushServiceClientException ex) when (ex.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Gone)
            {
                gone.Add(subscription.Id);
            }
            catch (Exception ex) when (ex is PushServiceClientException or HttpRequestException)
            {
                logger.LogWarning(ex, "Push to {Host} failed", new Uri(subscription.Endpoint).Host);
            }
        }

        if (gone.Count > 0)
        {
            await using var db = await dbFactory.CreateDbContextAsync(ct);
            await db.PushSubscriptions.Where(s => gone.Contains(s.Id)).ExecuteDeleteAsync(ct);
            logger.LogInformation("Removed {Count} expired push subscription(s)", gone.Count);
        }
    }

    private static WebPushSubscription ToWebPush(Data.PushSubscription s) => new()
    {
        Endpoint = s.Endpoint,
        Keys = new Dictionary<string, string> { ["p256dh"] = s.P256dh, ["auth"] = s.Auth }
    };

    private async Task<VapidKey> GetKeyAsync(CancellationToken ct)
    {
        if (_key is not null)
            return _key;

        await _keyLock.WaitAsync(ct);
        try
        {
            if (_key is not null)
                return _key;

            await using var db = await dbFactory.CreateDbContextAsync(ct);
            var key = await db.VapidKeys.AsNoTracking().FirstOrDefaultAsync(ct);
            if (key is null)
            {
                key = CreateKey();
                db.VapidKeys.Add(key);
                await db.SaveChangesAsync(ct);
                logger.LogInformation("Created the VAPID key pair for Web Push");
            }

            return _key = key;
        }
        finally
        {
            _keyLock.Release();
        }
    }

    /// <summary>P-256 pair in the format browsers expect: uncompressed public point and raw private scalar, base64url.</summary>
    public static VapidKey CreateKey()
    {
        using var ec = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        var p = ec.ExportParameters(includePrivateParameters: true);
        return new VapidKey
        {
            Id = 1,
            PublicKey = Base64Url.EncodeToString([0x04, .. p.Q.X!, .. p.Q.Y!]),
            PrivateKey = Base64Url.EncodeToString(p.D)
        };
    }
}
