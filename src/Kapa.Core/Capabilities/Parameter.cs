using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Rules;

namespace Kapa.Core.Capabilities;

public record Parameter : IParameter
{
    public Parameter(
        string name,
        string description,
        ParameterTypes kapaParamTypes,
        params IRule[] rules
    )
    {
        Name = name;
        Description = description;
        ParameterType = kapaParamTypes;
        Rules = rules;
    }

    public string Name { get; init; }
    public string Description { get; init; }
    public ParameterTypes ParameterType { get; init; }
    public IReadOnlyCollection<IRule> Rules { get; init; }
}
