using Kapa.Abstractions.Actors;

namespace Kapa.Abstractions.BDD;

public interface IScenarioBuilder
{
    public IActor Actor { get; }

    public T GetCapabilityType<T>();

    public void AddSteps(IKapaStep step);
}
