using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Extensions;

public static class NodeExtensions
{
    public static INode GetByName(this ICollection<INode> nodes, string methodName)
    {
        ArgumentNullException.ThrowIfNull(nodes);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(methodName);
        var capabilitiesAsNodes = nodes.Where(x => x.Capability.Name == methodName).ToArray();

        return GetMatchingNodeOrThrow(capabilitiesAsNodes, methodName);
    }

    public static INode GetByCapabilitySouce<TCapabilityType>(
        this ICollection<INode> nodes,
        string methodName,
        params Type[] parameterTypes
    )
    {
        ArgumentNullException.ThrowIfNull(nodes);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(methodName);
        ArgumentNullException.ThrowIfNull(parameterTypes);

        var source = InferSourceName<TCapabilityType>(methodName, parameterTypes);

        var capabilitiesAsNodes = nodes
            .Where(x => x.Capability.OutcomeMetadata.Source == source)
            .ToArray();

        return GetMatchingNodeOrThrow(capabilitiesAsNodes, methodName, parameterTypes);
    }

    public static string InferSourceName<T>(string methodName, params Type[] parameterTypes)
    {
        var method = typeof(T).GetMethod(methodName, parameterTypes);
        return method is null
            ? throw new MissingMethodException(typeof(T).FullName, methodName)
            : method.InferSourceName();
    }

    private static INode GetMatchingNodeOrThrow(
        INode[] capabilitiesAsNodes,
        string methodName,
        params Type[] parameterTypes
    )
    {
        if (capabilitiesAsNodes.Length == 1)
        {
            return capabilitiesAsNodes.First();
        }

        var parameters = string.Join(';', parameterTypes.Select(x => x.Name));
        var parameterMessage =
            parameterTypes.Length > 0 ? $" with {nameof(parameterTypes)} '{parameters}'" : "";

        var methodSignatureMessage = $" the method signature '{methodName}'{parameterMessage}";

        if (capabilitiesAsNodes.Length > 1)
        {
            throw new InvalidOperationException(
                $"Too many {nameof(ICapability.OutcomeMetadata.Source)}s match {methodSignatureMessage}."
            );
        }

        throw new InvalidOperationException(
            $"No {nameof(ICapability.OutcomeMetadata.Source)} match {methodSignatureMessage}."
        );
    }
}
