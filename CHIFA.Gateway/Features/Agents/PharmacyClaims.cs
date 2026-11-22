using System.Security.Claims;

namespace CHIFA.Gateway.Features.Agents;

public static class PharmacyClaims
{
    public const string PharmacyId = "pharmacy_id";

    public static Guid GetPharmacyId(this ClaimsPrincipal user) =>
        Guid.TryParse(user.FindFirstValue(PharmacyId), out var id)
            ? id
            : throw new InvalidOperationException("The user has no pharmacy_id claim.");
}
