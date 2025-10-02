using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Rules;

namespace Kapa.Core.Capabilities;

public record KapaParam : IKapaParam
{
    public KapaParam(
        string name,
        string description,
        KapaParamTypes kapaParamTypes,
        params IRule[] rules
    )
    {
        Name = name;
        Description = description;
        KapaParamType = kapaParamTypes;
        Rules = rules;
    }

    public string Name { get; init; }
    public string Description { get; init; }
    public KapaParamTypes KapaParamType { get; init; }
    public IReadOnlyCollection<IRule> Rules { get; init; }
}
