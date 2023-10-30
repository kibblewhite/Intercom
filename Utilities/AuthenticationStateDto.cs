using System.Security.Claims;

namespace Intercom.Utilities;

public sealed class AuthenticationStateDto
{
    /// <inheritdoc cref="ClaimsPrincipal.Claims" />
    public required IEnumerable<Claim> Claims { get; init; }

    /// <inheritdoc cref="ClaimsPrincipal.Identities" />
    public required IEnumerable<ClaimsIdentity> Identities { get; init; }

    /// <inheritdoc cref="ClaimsPrincipal.Identity" />
    public required System.Security.Principal.IIdentity? Identity { get; init; }
}
