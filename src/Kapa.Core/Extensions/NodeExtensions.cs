using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Graphs;

namespace Kapa.Core.Extensions;

public static class NodeExtensions
{
    public static INode ByCapabilitySouce<TCapabilityType>(
        this ICollection<INode> nodes,
        string methodName,
        params Type[] parameterTypes
    )
    {
        ArgumentNullException.ThrowIfNull(nodes);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(methodName);
        var source = InferSourceName<TCapabilityType>(methodName, parameterTypes);

        var capabilitiesAsNodes = nodes
            .Where(x => x.Capability.OutcomeMetadata.Source == source)
            .ToArray();

        if (capabilitiesAsNodes.Length == 1)
        {
            return capabilitiesAsNodes.First();
        }

        var parameters = string.Join(';', parameterTypes.Select(x => x.Name));
        var methodSignatureMessage =
            $" the method signature '{methodName}' with {nameof(parameterTypes)} '{parameters}'";

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

    public static string InferSourceName<T>(string methodName, params Type[] parameterTypes)
    {
        var method = typeof(T).GetMethod(methodName, parameterTypes);
        return method is null
            ? throw new MissingMethodException(typeof(T).FullName, methodName)
            : method.InferSourceName();
    }
}
