using Kapa.Abstractions.Rules;

namespace Kapa.Abstractions.Capabilities;

/// <summary>
/// A <see cref="ICapability"/> parameter.
/// </summary>
public interface IParameter
{
    public string Name { get; }
    public string Description { get; }

    /// <summary>
    /// The <see cref="IRule"/> applied to this <see cref="IParameter"/>.
    /// Usually set to define business rules.
    /// </summary>
    public IReadOnlyCollection<IRule> Rules { get; }
    public ParameterTypes ParameterType { get; }
}
