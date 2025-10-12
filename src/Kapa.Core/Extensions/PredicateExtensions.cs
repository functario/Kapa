using Kapa.Abstractions.Actors;
using Kapa.Core.Factories;

namespace Kapa.Core.Extensions;

public static class PredicateExtensions
{
    public static IRequirement<TGeneratedActor> ToRequirement<TGeneratedActor>(
        this Func<TGeneratedActor, bool> predicate,
        string description
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return RequirementFactory.Create(predicate, description);
    }

    public static IMutation<TGeneratedActor> ToMutation<TGeneratedActor>(
        this Func<TGeneratedActor, bool> predicate,
        string description
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return MutationFactory.Create(predicate, description);
    }

    public static bool ArePredicatesEquivalent(
        this Func<IGeneratedActor, bool> predicate1,
        Func<IGeneratedActor, bool> predicate2
    )
    {
        ArgumentNullException.ThrowIfNull(predicate1);
        ArgumentNullException.ThrowIfNull(predicate2);
        // Direct reference equality
        if (predicate1 == predicate2)
            return true;

        // Compare delegate targets and methods
        // Both predicates are wrappers like: actor => originalPredicate((TGeneratedActor)actor)
        // We need to check if they wrap the same original predicate

        if (predicate1.Target == null || predicate2.Target == null)
            return false;

        // Get the fields of the closure objects (the lambda closures)
        var fields1 = predicate1
            .Target.GetType()
            .GetFields(
                System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.Instance
            );

        var fields2 = predicate2
            .Target.GetType()
            .GetFields(
                System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.Instance
            );

        // Look for the captured predicate in the closure
        foreach (var field1 in fields1)
        {
            if (
                field1.FieldType.IsGenericType
                && field1.FieldType.GetGenericTypeDefinition() == typeof(Func<,>)
            )
            {
                var capturedFunc1 = field1.GetValue(predicate1.Target) as Delegate;

                foreach (var field2 in fields2)
                {
                    if (
                        field2.FieldType.IsGenericType
                        && field2.FieldType.GetGenericTypeDefinition() == typeof(Func<,>)
                    )
                    {
                        var capturedFunc2 = field2.GetValue(predicate2.Target) as Delegate;

                        if (capturedFunc1 != null && capturedFunc2 != null)
                        {
                            // Compare the captured delegates
                            if (
                                capturedFunc1.Target == capturedFunc2.Target
                                && capturedFunc1.Method == capturedFunc2.Method
                            )
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}
