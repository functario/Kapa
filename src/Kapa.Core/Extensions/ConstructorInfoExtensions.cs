using System.Reflection;
using Kapa.Core.Prototypes;

namespace Kapa.Core.Extensions;

internal static class ConstructorInfoExtensions
{
    internal static bool IsTraitConstructor(this ConstructorInfo constructor) =>
        constructor?.IsDefined(typeof(TraitConstructorAttribute), inherit: true) ?? false;
}
