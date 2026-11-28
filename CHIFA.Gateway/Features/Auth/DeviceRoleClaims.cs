using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CHIFA.Gateway.Features.Auth;

/// <summary>
/// What a phone may see (<see cref="DeviceRoles"/>). The gateway is where it is enforced: hiding a page in the app
/// would not stop an employee calling the API with the phone's token.
/// </summary>
public static class DeviceRoleClaims
{
    public const string Claim = "device_role";
    public const string OwnerPolicy = "owner";

    /// <summary>Tokens issued before roles, and password sign-ins, are the pharmacist's own.</summary>
    public static string GetDeviceRole(this ClaimsPrincipal user) => user.FindFirstValue(Claim) ?? DeviceRoles.Owner;

    public static bool IsStaff(this ClaimsPrincipal user) => user.GetDeviceRole() == DeviceRoles.Staff;

    public static void AddOwnerPolicy(this AuthorizationOptions options) =>
        options.AddPolicy(OwnerPolicy, p => p.RequireAuthenticatedUser().RequireAssertion(c => !c.User.IsStaff()));

    /// <summary>Turnover, margins, bordereaux, payments, alerts and the pharmacy's account: not for an employee's phone.</summary>
    public static TBuilder RequireOwner<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder =>
        builder.RequireAuthorization(OwnerPolicy);
}
