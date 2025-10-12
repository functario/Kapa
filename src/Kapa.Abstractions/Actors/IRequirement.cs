namespace Kapa.Abstractions.Actors;

public interface IRequirement<out TGeneratedActor>
    where TGeneratedActor : IGeneratedActor
{
    public string Description { get; }
    public Func<IGeneratedActor, bool> Predicate { get; }
}
