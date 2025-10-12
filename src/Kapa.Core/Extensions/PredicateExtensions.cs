using Kapa.Abstractions.Actors;
using Kapa.Core.Factories;

namespace Kapa.Core.Extensions;

public static class PredicateExtensions
{
    public static IEffect<TGeneratedActor> ToEffect<TGeneratedActor>(
        this Func<TGeneratedActor, bool> predicate,
        string description
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(predicate);

        return EffectFactory.Create(predicate, description);
    }

    /// <summary>
    /// Determines whether two predicates are functionally equivalent by comparing their underlying implementation.
    /// This method is designed to match predicates that have been wrapped by factory methods like
    /// <see cref="EffectFactory.Create{TGeneratedActor}"/>.
    /// </summary>
    /// <param name="predicate1">The first predicate to compare.</param>
    /// <param name="predicate2">The second predicate to compare.</param>
    /// <returns><see langword="true"/> if the predicates are equivalent; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// <para>
    /// This method supports three comparison strategies:
    /// </para>
    /// <list type="number">
    /// <item>
    /// <term>Reference Equality</term>
    /// <description>
    /// Fast path for shared predicate instances, such as static properties.
    /// <code>
    /// // Example: Static predicate properties
    /// public static Func&lt;IUser, bool&gt; IsAuthenticated => u => u.Identification != null;
    ///
    /// var req = IUser.IsAuthenticated.ToRequirement("Is authenticated");
    /// var mut = IUser.IsAuthenticated.ToMutation("Is authenticated");
    /// // These will match via reference equality
    /// </code>
    /// </description>
    /// </item>
    /// <item>
    /// <term>Captured Delegate Comparison</term>
    /// <description>
    /// Compares the delegates captured in closure objects when predicates wrap the same underlying function.
    /// <code>
    /// // Example: Reused lambda variable
    /// Func&lt;ActorA, bool&gt; sharedPredicate = a => a.StateAsInt > 2;
    ///
    /// var req = RequirementFactory.Create(sharedPredicate, "State > 2");
    /// var mut = MutationFactory.Create(sharedPredicate, "State > 2");
    /// // These will match via captured delegate comparison
    /// </code>
    /// </description>
    /// </item>
    /// <item>
    /// <term>IL Bytecode Comparison</term>
    /// <description>
    /// Compares the compiled IL bytecode for compiler-generated lambdas with identical logic but different instances.
    /// <code>
    /// // Example: Inline lambdas with identical logic
    /// var req = RequirementFactory.Create&lt;ActorA&gt;(p => p.IsStateTrue == true, "description");
    /// var mut = MutationFactory.Create&lt;ActorA&gt;(p => p.IsStateTrue == true, "description");
    /// // These will match via IL bytecode comparison
    /// </code>
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// <strong>Note:</strong> IL bytecode comparison works for simple lambda expressions but may not match
    /// complex expressions that produce different IL despite having the same semantic meaning.
    /// For reliable matching across different code locations, use shared predicate instances.
    /// </para>
    /// </remarks>
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
                foreach (var field2 in fields2)
                {
                    if (
                        field2.FieldType.IsGenericType
                        && field2.FieldType.GetGenericTypeDefinition() == typeof(Func<,>)
                    )
                    {
                        if (
                            field1.GetValue(predicate1.Target) is Delegate capturedFunc1
                            && field2.GetValue(predicate2.Target) is Delegate capturedFunc2
                        )
                        {
                            // First try direct comparison
                            if (
                                capturedFunc1.Target == capturedFunc2.Target
                                && capturedFunc1.Method == capturedFunc2.Method
                            )
                            {
                                return true;
                            }

                            // If both are compiler-generated lambdas, compare their IL byte code
                            if (
                                capturedFunc1.Method.DeclaringType?.Name.Contains(
                                    "<>c",
                                    StringComparison.Ordinal
                                ) == true
                                && capturedFunc2.Method.DeclaringType?.Name.Contains(
                                    "<>c",
                                    StringComparison.Ordinal
                                ) == true
                            )
                            {
#pragma warning disable CA1031 // Do not catch general exception types
                                try
                                {
                                    var methodBody1 = capturedFunc1.Method.GetMethodBody();
                                    var methodBody2 = capturedFunc2.Method.GetMethodBody();

                                    if (methodBody1 != null && methodBody2 != null)
                                    {
                                        var il1 = methodBody1.GetILAsByteArray();
                                        var il2 = methodBody2.GetILAsByteArray();

                                        if (il1 != null && il2 != null && il1.SequenceEqual(il2))
                                        {
                                            return true;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    // If we can't get IL, continue
                                }
#pragma warning restore CA1031 // Do not catch general exception types
                            }
                        }
                    }
                }
            }
        }

        return false;
    }
}
