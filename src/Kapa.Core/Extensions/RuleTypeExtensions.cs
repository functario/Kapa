using Kapa.Abstractions.Rules;

namespace Kapa.Core.Extensions;

internal static class RuleTypeExtensions
{
    public static IRule? ToRule(this Type ruleType)
    {
        if (
            typeof(IRule).IsAssignableFrom(ruleType)
            && ruleType.GetConstructor(Type.EmptyTypes) is not null
            && Activator.CreateInstance(ruleType) is IRule instance
        )
        {
            return instance;
        }

        return null;
    }

    public static IEnumerable<IRule> ToRules(this IReadOnlyCollection<Type> ruleTypes)
    {
        foreach (var type in ruleTypes)
        {
            if (type.ToRule() is IRule instance)
            {
                yield return instance;
            }
            else
            {
                throw new InvalidCastException(
                    $"Could not cast type '{type.FullName}' to '{nameof(IRule)}'."
                );
            }
        }
    }
}
