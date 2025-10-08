using System.Reflection;
using Kapa.Abstractions.Exceptions;
using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;

namespace Kapa.Core.Extensions;

public static class PrototypeTypeExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="prototypeType"></param>
    /// <returns></returns>
    /// <exception cref="TypeIsNotPrototypeException"/>
    public static IPrototype ToPrototype(this Type prototypeType)
    {
        ArgumentNullException.ThrowIfNull(prototypeType);
        ThrowIfNotPrototypeType(prototypeType);

        var states = prototypeType.GetStates();

        var prototypeAttribute = prototypeType.GetPrototypeAttribute();

        return new Prototype(prototypeType.Name, prototypeAttribute.Description, [.. states]);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="prototypeType"></param>
    /// <returns></returns>
    /// <exception cref="TypeIsNotPrototypeException"/>
    public static PrototypeAttribute GetPrototypeAttribute(this Type prototypeType)
    {
        ArgumentNullException.ThrowIfNull(prototypeType);
        ThrowIfNotPrototypeType(prototypeType);
        return prototypeType
            .GetCustomAttributes(typeof(PrototypeAttribute), inherit: true)
            .Cast<PrototypeAttribute>()
            .Single();
    }

    public static ICollection<IState> GetStates(this Type prototypeType)
    {
        ArgumentNullException.ThrowIfNull(prototypeType);
        ThrowIfNotPrototypeType(prototypeType);

        var states = new List<IState>();

        var properties = prototypeType.GetProperties(
            BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.NonPublic
        );

        foreach (var property in properties)
        {
            var stateAttribute = property.GetStateAttribute();
            if (stateAttribute is not null)
            {
                states.Add(CreateState(stateAttribute, property));
            }
        }

        return states;
    }

    public static bool IsPrototypeType(this Type type) =>
        type?.IsDefined(typeof(PrototypeAttribute), inherit: true) ?? false;

    private static State CreateState(StateAttribute stateAttribute, PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(stateAttribute);
        ArgumentNullException.ThrowIfNull(property);
        var parameters = property.GetParameters();
        return new State(property.Name, stateAttribute.Description, [.. parameters]);
    }

    private static void ThrowIfNotPrototypeType(this Type type)
    {
        if (!type.IsPrototypeType())
        {
            throw new TypeIsNotPrototypeException(
                type,
                $"The attribute '{nameof(PrototypeAttribute)}' does not decorate the class."
            );
        }
    }
}
