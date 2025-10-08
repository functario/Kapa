using Kapa.Abstractions.Prototypes;

namespace Kapa.Core.Prototypes;

/// <summary>
/// Indicates that the constructor should be used to document the parameters of the <see cref="IState"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
public sealed class StateConstructorAttribute : Attribute { }
