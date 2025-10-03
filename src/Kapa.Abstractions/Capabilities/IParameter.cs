using Kapa.Abstractions.Rules;

namespace Kapa.Abstractions.Capabilities;

public interface IParameter
{
    public string Name { get; }
    public string Description { get; }
    public IReadOnlyCollection<IRule> Rules { get; }
    public ParameterTypes ParameterType { get; }
}
