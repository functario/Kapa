using Kapa.Abstractions.Actors;
using Kapa.Core.Actors;

namespace HomeAutomation.Actors.Identifications;

[Actor($"All realted {nameof(State)} to identify a user.")]
public sealed class Identification : IGeneratedActor
{
    [State($"The {nameof(User)} authentication state.")]
    public bool IsAuthenticated { get; set; }

    [State($"{nameof(User)} authentication token.")]
    public Token? Token { get; set; }
}
