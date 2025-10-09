using Kapa.Core.Prototypes;

namespace HomeAutomation.Prototypes.Identifications;

[Prototype($"All realted {nameof(State)} to identify a user.")]
public sealed class Identification : IGeneratedPrototype
{
    [State($"The {nameof(User)} authentication state.")]
    public bool IsAuthenticated { get; set; }

    [State($"{nameof(User)} authentication token.")]
    public Token? Token { get; set; }
}
