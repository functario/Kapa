using AwesomeAssertions.Execution;
using Kapa.Abstractions.Capabilities;
using Kapa.Abstractions.Outcomes;
using Kapa.Core.Extensions;
using Newtonsoft.Json;

namespace Workbench;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Act
        var capaA = typeof(CapaA).ToCapabilityType();
        var capaB = typeof(CapaB).ToCapabilityType();

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

    [Fact]
    public void MyTestMethod()
    {
        var capaA = new CapaA();
        var capaC = new CapaC();

        capaA.DoSomething().Should().BeAssignableTo<IOutcome>();
        var a = capaC.DoSomething();
        a.Should().BeAssignableTo<ITypedOutcome<string>>();
        a.As<ITypedOutcome<string>>().Value.Should().BeEquivalentTo("value");
    }
}
