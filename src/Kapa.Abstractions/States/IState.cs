namespace Kapa.Abstractions.States;

public interface IState
{
    string Name { get; }
    public string Description { get; }
    public IReadOnlyCollection<ITrait> Traits { get; }
}
