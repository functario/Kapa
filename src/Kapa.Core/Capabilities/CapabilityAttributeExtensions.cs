namespace Kapa.Core.Capabilities;

public static class CapabilityAttributeExtensions
{
    public static void ToCapability(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var _ = type.Name;
    }
}
