using AwesomeAssertions.Execution;
using Kapa.Abstractions.Capabilities;
using Kapa.Core.Extensions;
using Newtonsoft.Json;

namespace Workbench;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Act
        var capaA = typeof(CapaA).ToCapability();
        var capaB = typeof(CapaB).ToCapability();

        // Assert
        using var scope = new AssertionScope();

        var a = JsonConvert.SerializeObject(capaB);

        capaA
            .Should()
            .NotBeNull()
            .And.Satisfy<ICapabilityType>(k => k.Capabilities.Should().HaveCount(1));

        capaB
            .Should()
            .NotBeNull()
            .And.Satisfy<ICapabilityType>(k => k.Capabilities.Should().HaveCount(1));
    }
}
