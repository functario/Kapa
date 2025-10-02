using Kapa.Abstractions.Rules;

namespace Kapa.Abstractions.Capabilities;

public interface IKapaParam
{
    public string Name { get; }
    public string Description { get; }
    public IReadOnlyCollection<IRule> Rules { get; }
    public KapaParamTypes KapaParamType { get; }
}
