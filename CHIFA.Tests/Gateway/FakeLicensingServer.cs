using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CHIFA.Gateway.Features.LicenseRequests;

namespace CHIFA.Tests.Gateway;

/// <summary>
/// The license request routes of SmartApp.Licensing, kept in memory: one open request per computer, and
/// sending again replaces its token, like the real server. Tests use their own machine ids.
/// </summary>
public sealed class FakeLicensingServer : HttpMessageHandler
{
    private readonly ConcurrentDictionary<Guid, Request> _requests = new();
    private readonly ConcurrentDictionary<string, bool> _down = new();

    public sealed class Request(Guid id, LicensingSubmission submission)
    {
        public Guid Id { get; } = id;
        public LicensingSubmission Submission { get; set; } = submission;
        public string Token { get; set; } = Guid.NewGuid().ToString("N");
        public string Status { get; set; } = "pending";
        public string? Serial { get; set; }
        public string? Message { get; set; }
    }

    public Request? For(string machineId) => _requests.Values.SingleOrDefault(r => r.Submission.MachineId == machineId);

    public void Approve(string machineId, string serial) => (For(machineId)!.Status, For(machineId)!.Serial) = ("approved", serial);

    public void Reject(string machineId, string reason) => (For(machineId)!.Status, For(machineId)!.Message) = ("rejected", reason);

    /// <summary>What SmartCHIFA does when it sends the request again from the computer.</summary>
    public void ReplaceToken(string machineId) => For(machineId)!.Token = Guid.NewGuid().ToString("N");

    /// <summary>Answers 503 for this computer, as an unreachable or failing server would.</summary>
    public void SetDown(string machineId) => _down[machineId] = true;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var path = request.RequestUri!.AbsolutePath.TrimEnd('/');
        if (path.EndsWith("/api/v1/requests/status"))
        {
            var query = (await request.Content!.ReadFromJsonAsync<LicensingStatusQuery>(JsonSerializerOptions.Web, ct))!;
            if (_down.ContainsKey(query.MachineId))
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            if (!_requests.TryGetValue(query.Id, out var found) || found.Token != query.Token || found.Submission.MachineId != query.MachineId)
                return Json(HttpStatusCode.NotFound, new LicensingError(LicensingError.RequestNotFound, "This request does not exist. Send a new one."));
            return Json(HttpStatusCode.OK, new LicensingStatus(found.Status, found.Serial is null ? null : "SL1.license", found.Serial, found.Message));
        }

        if (path.EndsWith("/api/v1/requests"))
        {
            var submission = (await request.Content!.ReadFromJsonAsync<LicensingSubmission>(JsonSerializerOptions.Web, ct))!;
            if (_down.ContainsKey(submission.MachineId))
                return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);

            var open = _requests.Values.SingleOrDefault(r => r.Submission.MachineId == submission.MachineId && r.Status == "pending");
            if (open is null)
            {
                open = new Request(Guid.NewGuid(), submission);
                _requests[open.Id] = open;
            }
            else
            {
                open.Submission = submission;
                open.Token = Guid.NewGuid().ToString("N");
            }

            return Json(HttpStatusCode.OK, new LicensingReceipt(open.Id, open.Token));
        }

        return new HttpResponseMessage(HttpStatusCode.NotFound);
    }

    private static HttpResponseMessage Json<T>(HttpStatusCode status, T body) =>
        new(status) { Content = JsonContent.Create(body, options: JsonSerializerOptions.Web) };
}
