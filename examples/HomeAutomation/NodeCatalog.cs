using System.Reflection;
using System.Runtime.CompilerServices;
using Kapa.Abstractions.Graphs;
using Kapa.Core.Extensions;

namespace HomeAutomation;

public static class NodeCatalog
{
    private static INode[]? s_nodes;

    public static ICollection<INode> Nodes
    {
        get
        {
            s_nodes ??= GetNodes();
            return s_nodes;
        }
    }

    private static INode[] GetNodes()
    {
        var capabilityTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<CapabilityTypeAttribute>() != null);

        var nodes = capabilityTypes.SelectMany(t => t.GetCapabilitiesAsNodes()).ToArray();

        return nodes;
    }

    public static INode SwitchLight => GetNode();
    public static INode SetThermostatSetpoint => GetNode();
    public static INode AddThermostat => GetNode();
    public static INode AddLight => GetNode();
    public static INode Setup => GetNode();
    public static INode AuthenticateAsync => GetNode();

    private static INode GetNode([CallerMemberName] string? caller = null)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(caller);
        return Nodes.GetByName(caller);
    }
}
