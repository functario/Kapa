using System.Reflection;
using Kapa.Core.Prototypes;

namespace Kapa.Core.Extensions;

internal static class ConstructorInfoExtensions
{
    internal static bool IsStateConstructor(this ConstructorInfo constructor) =>
        constructor?.IsDefined(typeof(StateConstructorAttribute), inherit: true) ?? false;
}
