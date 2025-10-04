using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Exceptions;
using Kapa.Abstractions.States;
using Kapa.Core.Capabilities;
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

    private static List<IParameter> GetParametersFromProperty(PropertyInfo property)
    {
        var propertyType = property.PropertyType;

        // If the property type is a record or class, check its constructor parameters
        var constructor = propertyType.GetConstructors().FirstOrDefault();
        if (constructor is null)
        {
            return [];
        }

        var parameters = new List<IParameter>();
        var constructorParams = constructor.GetParameters();

        foreach (var param in constructorParams)
        {
            var paramAttr = param
                .GetCustomAttributes(typeof(ParameterAttribute), inherit: true)
                .Cast<ParameterAttribute>()
                .FirstOrDefault();

            if (paramAttr is not null)
            {
                parameters.Add(paramAttr.ToParameter(param));
            }
        }

        return parameters;
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
        var parameters = GetParametersFromProperty(property);
        return new Trait(property.Name, traitAttribute.Description, [.. parameters]);
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
