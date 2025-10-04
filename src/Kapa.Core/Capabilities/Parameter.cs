using Kapa.Abstractions;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Rules;

namespace Kapa.Core.Capabilities;

/// <inheritdoc/>
public record Parameter : IParameter
{
    public Parameter(string name, string description, SupportedKinds kinds, params IRule[] rules)
    {
        Name = name;
        Description = description;
        Kind = kinds;
        Rules = rules;
    }

    public string Name { get; init; }
    public string Description { get; init; }
    public SupportedKinds Kind { get; init; }
    public IReadOnlyCollection<IRule> Rules { get; init; }
}
