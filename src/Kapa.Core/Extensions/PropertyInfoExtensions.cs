using System.Diagnostics;
using System.Reflection;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Exceptions;
using Kapa.Abstractions.Prototypes;
using Kapa.Core.Capabilities;
using Kapa.Core.Prototypes;

namespace Kapa.Core.Extensions;

internal static class PropertyInfoExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    /// <exception cref="MultipleStateConstructorsException"/>
    /// <exception cref="UnreachableException"/>
    public static ConstructorInfo? GetStateConstructor(this PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(property.DeclaringType);
        var propertyType = property.PropertyType;
        var constructors = propertyType.GetConstructors();

        if (constructors.Length == 0)
        {
            return null;
        }

        if (constructors.Length == 1)
        {
            return constructors.First();
        }

        // In case of multiple constructors
        // find the single one with StateConstructorAttribute.
        // Throw if many or none.
        if (constructors.Length > 1)
        {
            var stateConstructors = constructors
                .Where(constructor => constructor.IsStateConstructor())
                .ToArray();

            if (stateConstructors.Length != 1)
            {
                throw new MultipleStateConstructorsException(
                    property.DeclaringType,
                    $"Add the attribute '{nameof(StateConstructorAttribute)}' to the constructor"
                        + $" used to document the parameters of the {nameof(IState)}."
                );
            }

            return stateConstructors.First();
        }

        throw new UnreachableException(
            $"Could not defined the constructor "
                + $"used to document the parameters of the {nameof(IState)}."
        );
    }

    public static List<IParameter> GetParameters(this PropertyInfo property)
    {
        var constructor = property.GetStateConstructor();
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

    public static StateAttribute? GetStateAttribute(this PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(property);
        return property
            .GetCustomAttributes(typeof(StateAttribute), inherit: true)
            .Cast<StateAttribute>()
            .FirstOrDefault();
    }
}
