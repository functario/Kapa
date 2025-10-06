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
        ThrowIfNotStateType(prototypeType);

        var traits = prototypeType.GetTraits();

        var stateAttribute = prototypeType.GetStateAttribute();

        return new Prototype(prototypeType.Name, stateAttribute.Description, [.. traits]);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="prototypeType"></param>
    /// <returns></returns>
    /// <exception cref="TypeIsNotPrototypeException"/>
    public static PrototypeAttribute GetStateAttribute(this Type prototypeType)
    {
        ArgumentNullException.ThrowIfNull(prototypeType);
        ThrowIfNotStateType(prototypeType);
        return prototypeType
            .GetCustomAttributes(typeof(PrototypeAttribute), inherit: true)
            .Cast<PrototypeAttribute>()
            .Single();
    }

    public static ICollection<ITrait> GetTraits(this Type prototypeType)
    {
        ArgumentNullException.ThrowIfNull(prototypeType);
        ThrowIfNotStateType(prototypeType);

        var traits = new List<ITrait>();

        var properties = prototypeType.GetProperties(
            BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.Static
                | BindingFlags.NonPublic
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
        type?.IsDefined(typeof(PrototypeAttribute), inherit: true) ?? false;

    private static Trait CreateTrait(TraitAttribute traitAttribute, PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(traitAttribute);
        ArgumentNullException.ThrowIfNull(property);
        var parameters = property.GetParameters();
        return new Trait(property.Name, traitAttribute.Description, [.. parameters]);
    }

    private static void ThrowIfNotStateType(this Type type)
    {
        if (!type.IsStateType())
        {
            throw new TypeIsNotPrototypeException(
                type,
                $"The attribute '{nameof(PrototypeAttribute)}' does not decorate the class."
            );
        }
    }
}
