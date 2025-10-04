using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Exceptions;
using Kapa.Abstractions.States;
using Kapa.Core.Capabilities;
using Kapa.Core.States;

namespace Kapa.Core.Extensions;

internal static class PropertyInfoExtensions
{
    public static ConstructorInfo? GetTraitConstructor(this PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(property.DeclaringType);
        var propertyType = property.PropertyType;
        // If the property type is a record or class, check its constructor parameters
        var constructors = propertyType.GetConstructors();

        if (constructors.Length == 0)
        {
            return null;
        }

        ConstructorInfo? constructor = null;
        if (constructors.Length == 1)
        {
            constructor = constructors.First();
        }

        // In case of multiple constructors
        // find the single one with TraitConstructorAttribute.
        // Throw if many or none.
        if (constructors.Length > 1)
        {
            var traitConstructors = constructors
                .Where(constructor => constructor.IsTraitConstructor())
                .ToArray();

            if (traitConstructors.Length != 1)
            {
                throw new MultipleTraitConstructorsException(
                    property.DeclaringType,
                    $"Add the attribute '{nameof(TraitConstructorAttribute)}' to the constructor"
                        + $" used to document the parameters of the {nameof(ITrait)}."
                );
            }

            constructor = traitConstructors.First();
        }

        if (constructor is null)
        {
            throw new InvalidOperationException(
                $"Could not defined the constructor "
                    + $"used to document the parameters of the {nameof(ITrait)}."
            );
        }

        return constructor;
    }

    public static List<IParameter> GetParameters(this PropertyInfo property)
    {
        var constructor = property.GetTraitConstructor();
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

    public static TraitAttribute? GetTraitAttribute(this PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(property);
        return property
            .GetCustomAttributes(typeof(TraitAttribute), inherit: true)
            .Cast<TraitAttribute>()
            .FirstOrDefault();
    }
}
