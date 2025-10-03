namespace Kapa.Fixtures.Capabilities;

[CapabilityType]
public sealed class EmptyCapabilityType
{
    // Intentionally has no methods decorated with [Capability] attribute for testing
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Performance",
        "CA1822:Mark members as static",
        Justification = "For testing"
    )]
    public void Handle()
    {
        // This method has no [Capability] attribute
    }
}
