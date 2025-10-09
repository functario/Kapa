using Kapa.Core.Capabilities;

namespace HomeAutomation.Prototypes.Identifications;

public sealed record Token(
    [Parameter($"{nameof(User)} access token.")] string AccessToken,
    [Parameter($"{nameof(User)} refresh token.")] string RefreshToken,
    [Parameter($"{nameof(User)} refresh token expiration DateTimeOffset.")] DateTimeOffset ExpiresOn
)
{ }
