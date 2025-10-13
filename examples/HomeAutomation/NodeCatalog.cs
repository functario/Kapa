using System.Reflection;
using System.Runtime.CompilerServices;
using Kapa.Abstractions.Graphs;
using Kapa.Core.Extensions;

namespace HomeAutomation;

public static class NodeCatalog
{
    public static INode[] GetNodes()
    {
        var capabilityTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetCustomAttribute<CapabilityTypeAttribute>() != null)
            .ToArray();

        var nodes = capabilityTypes.SelectMany(t => t.GetCapabilitiesAsNodes()).ToArray();

        // Debug: Verify nodes are being collected
        if (nodes.Length == 0)
        {
            throw new InvalidOperationException("GetNodes() returned zero nodes!");
        }

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
        return GetNodes().GetByName(caller);
    }
}
