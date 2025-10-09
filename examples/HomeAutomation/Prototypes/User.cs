using HomeAutomation.Prototypes.Homes;
using HomeAutomation.Prototypes.Identifications;
using Kapa.Core.Prototypes;

namespace HomeAutomation.Prototypes;

[Prototype($"The {nameof(User)}.")]
public sealed record User : IGeneratedPrototype, IUser
{
    [State($"All realted {nameof(State)} to identify a user.")]
    public Identification? Identification { get; set; }

    [State($"Information about the user {nameof(Homes.Home)}.")]
    public Home? Home { get; set; }
}
