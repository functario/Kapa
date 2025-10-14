using System.Reflection;
using Kapa.Abstractions.Actors;
using Kapa.Abstractions.Exceptions;
using Kapa.Core.Actors;

namespace Kapa.Core.Extensions;

public static class ActorTypeExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="actorType"></param>
    /// <returns></returns>
    /// <exception cref="TypeIsNotActorException"/>
    public static IActor ToActor(this Type actorType)
    {
        ArgumentNullException.ThrowIfNull(actorType);
        ThrowIfNotActorType(actorType);

        var states = actorType.GetStates();

        var actorAttribute = actorType.GetActorAttribute();

        return new Actor(actorType.Name, actorAttribute.Description, [.. states]);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="actorType"></param>
    /// <returns></returns>
    /// <exception cref="TypeIsNotActorException"/>
    public static ActorAttribute GetActorAttribute(this Type actorType)
    {
        ArgumentNullException.ThrowIfNull(actorType);
        ThrowIfNotActorType(actorType);
        return actorType
            .GetCustomAttributes(typeof(ActorAttribute), inherit: true)
            .Cast<ActorAttribute>()
            .Single();
    }

    public static ICollection<IState> GetStates(this Type actorType)
    {
        ArgumentNullException.ThrowIfNull(actorType);
        ThrowIfNotActorType(actorType);

        var states = new List<IState>();

        var properties = actorType.GetProperties(
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

    public static bool IsActorType(this Type type) =>
        type?.IsDefined(typeof(ActorAttribute), inherit: true) ?? false;

    private static State CreateState(StateAttribute stateAttribute, PropertyInfo property)
    {
        ArgumentNullException.ThrowIfNull(stateAttribute);
        ArgumentNullException.ThrowIfNull(property);
        var parameters = property.GetParameters();
        return new State(property.Name, stateAttribute.Description, [.. parameters]);
    }

    private static void ThrowIfNotActorType(this Type type)
    {
        if (!type.IsActorType())
        {
            throw new TypeIsNotActorException(
                type,
                $"The attribute '{nameof(ActorAttribute)}' does not decorate the class."
            );
        }
    }
}
