namespace HomeAutomation.Actors;

[Actor($"The {nameof(User)}.")]
public sealed record User : IUser
{
    public User()
    {
        Home = new Home([]);
        Identification = new Identification();
    }

    [State($"All realted {nameof(State)} to identify a user.")]
    public Identification Identification { get; set; }

    [State($"Information about the user {nameof(Homes.Home)}.")]
    public Home Home { get; set; }
}
