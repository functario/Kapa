using System.Reflection;
using Kapa.Abstractions.Exceptions;
using Kapa.Abstractions.States;
using Kapa.Core.States;

namespace Kapa.Core.Extensions;

public static class StateTypeExtensions
{
    public static IState ToState(this Type stateType)
    {
        ArgumentNullException.ThrowIfNull(stateType);
        ThrowIfNotStateType(stateType);

        var traits = stateType.GetTraits();

        var stateAttribute = stateType.GetStateAttribute();

        return new State(stateType.Name, stateAttribute.Description, [.. traits]);
    }

    public static StateAttribute GetStateAttribute(this Type stateType)
    {
        ArgumentNullException.ThrowIfNull(stateType);
        ThrowIfNotStateType(stateType);
        return stateType
            .GetCustomAttributes(typeof(StateAttribute), inherit: true)
            .Cast<StateAttribute>()
            .Single();
    }

    public static ICollection<ITrait> GetTraits(this Type stateType)
    {
        ArgumentNullException.ThrowIfNull(stateType);
        ThrowIfNotStateType(stateType);

        var traits = new List<ITrait>();

        var properties = stateType.GetProperties(
            BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic
                | BindingFlags.Static
        );

        foreach (var property in properties)
        {
            var traitAttribute = property.GetTraitAttribute();
            if (traitAttribute is not null)
            {
                traits.Add(CreateTrait(traitAttribute, property));
            }
        }

        return traits;
    }

    public static bool IsStateType(this Type type) =>
        type?.IsDefined(typeof(StateAttribute), inherit: true) ?? false;

    private static TraitAttribute? GetTraitAttribute(this PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(property);
        return property
            .GetCustomAttributes(typeof(TraitAttribute), inherit: true)
            .Cast<TraitAttribute>()
            .FirstOrDefault();
    }

    private static Trait CreateTrait(TraitAttribute traitAttribute, PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(traitAttribute);
        ArgumentNullException.ThrowIfNull(property);
        return new Trait(property.Name, traitAttribute.Description);
    }

    private static void ThrowIfNotStateType(this Type type)
    {
        if (!type.IsStateType())
        {
            throw new TypeIsNotStateException(
                type,
                $"The attribute '{nameof(StateAttribute)}' does not decorate the class."
            );
        }
    }
}
