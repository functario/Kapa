using Kapa.Abstractions.States;

namespace Kapa.Core.States;

/// <summary>
/// Indicates that the constructor should be used to document the parameters of the <see cref="ITrait"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = true)]
public sealed class TraitConstructorAttribute : Attribute { }
