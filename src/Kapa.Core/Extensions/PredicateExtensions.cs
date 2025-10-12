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
        // Default origin is TGeneratedActor
        return ToEffect<TGeneratedActor>(predicate, typeof(TGeneratedActor));
    }

    public static IEffect<TGeneratedActor> ToEffect<TGeneratedActor, TPredicateOrigin>(
        this Predicate<TGeneratedActor> predicate
    )
        where TGeneratedActor : IGeneratedActor
    {
        return ToEffect<TGeneratedActor>(predicate, typeof(TPredicateOrigin));
    }

    public static IEffect<TGeneratedActor> ToEffect<TGeneratedActor>(
        this Predicate<TGeneratedActor> predicate,
        Type predicateOrigin
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(predicateOrigin);
        var type = predicateOrigin;
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
            $"No {nameof(EffectPredicateAttribute)} found for predicate {predicateMethod.Name} on {type.Name}."
        );
    }
}
