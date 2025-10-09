using HomeAutomation.Actors.Homes;
using Kapa.Core.Actors;

namespace HomeAutomation.Actors;

[Actor($"The {nameof(User)}.")]
public sealed record User : IGeneratedActor, IUser
{
    [State($"All realted {nameof(State)} to identify a user.")]
    public Identification? Identification { get; set; }

    [State($"Information about the user {nameof(Homes.Home)}.")]
    public Home? Home { get; set; }
}
