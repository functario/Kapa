using Kapa.Abstractions.Actors;
using Kapa.Core.Actors;
using Kapa.Core.Factories;

namespace Kapa.Core.Extensions;

public static class PredicateExtensions
{
    public static IEffect<TGeneratedActor> ToEffect<TGeneratedActor>(
        this Predicate<TGeneratedActor> predicate
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(predicate);
        // Use reflection to find EffectPredicateAttribute on the property that returns this predicate
        var type = typeof(TGeneratedActor);
        var predicateMethod = predicate.Method;

        foreach (
            var prop in type.GetProperties(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
            )
        )
        {
            if (prop.PropertyType == typeof(Predicate<TGeneratedActor>))
            {
                if (
                    prop.GetValue(null) is Predicate<TGeneratedActor> value
                    && value.Method == predicateMethod
                )
                {
                    var attr = prop.GetCustomAttributes(typeof(EffectPredicateAttribute), true)
                        .OfType<EffectPredicateAttribute>()
                        .FirstOrDefault();

                    if (attr != null)
                    {
                        return EffectFactory.Create(predicate, attr.Id, attr.Description);
                    }
                }
            }
        }

        throw new InvalidOperationException(
            $"No EffectPredicateAttribute found for predicate {predicateMethod.Name} on {type.Name}."
        );
    }
}
