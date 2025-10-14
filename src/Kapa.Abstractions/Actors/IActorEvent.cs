namespace Kapa.Abstractions.Actors;

public interface IActorEvent
{
    /// <summary>
    /// The name of the <see cref="IActorEvent"/>.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The name of the <see cref="IActorEvent"/> sender.
    /// </summary>
    public string Sender { get; }

    /// <summary>
    /// The value of the event.
    /// </summary>
    public object Value { get; }
}
