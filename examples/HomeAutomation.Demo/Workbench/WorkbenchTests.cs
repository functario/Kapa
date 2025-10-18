using Microsoft.Extensions.Hosting;

namespace HomeAutomation.Demo.Workbench;

public class WorkbenchTests
{
    [Fact(
        DisplayName = $"{nameof(User)} is a shared instance between {nameof(ICapability)}.",
        Explicit = true
    )]
    public async Task Test1()
    {
        // Arrange
        var user = new User();
        var timeProvider = TimeProvider.System;
        var setups = new SetupUserCapabilities();
        var authentications = new AuthenticationCapabilities(timeProvider);
        var domotic = new DomoticCapabilities();
        var tstat00Name = "Thermostat00";
        var expectedSetpoint = 17.5;

        // Act
        setups.SetupUser(user);
        var sut1 = await authentications.AuthenticateAsync(
            user,
            "user@home.com",
            "1234!",
            CancellationToken.None
        );
        var sut2 = await domotic.SetThermostatSetpoint(
            user,
            tstat00Name,
            expectedSetpoint,
            CancellationToken.None
        );

        // Assert
        using var scope = new AssertionScope();
        var userOutcome1 = sut1.Outcome.As<Ok<IUser>>().Value;
        var userOutcome2 = sut2.Outcome.As<Ok<IUser>>().Value;

        // The same user all along.
        user.Should().BeSameAs(userOutcome1).And.BeSameAs(userOutcome2).And.NotBe(null);

        // Validate Thermostat changes.
        var tstat00 = user.Home?.GetDevice<Thermostat>(tstat00Name);
        tstat00.Should().NotBeNull();
        tstat00.Setpoint.Should().Be(expectedSetpoint);

        // Validate Authentication.
        var identification = sut1.Outcome.As<Ok<IUser>>().Value?.Identification;
        identification.Should().NotBeNull();
        identification!.IsAuthenticated.Should().BeTrue();
        identification!.Token?.AccessToken.Should().NotBeNullOrEmpty();
        identification!.Token?.RefreshToken.Should().NotBeNullOrEmpty();
        identification!.Token?.ExpiresOn.Should().BeAfter(timeProvider.GetUtcNow());
    }

    [Fact(DisplayName = $"Resolve {nameof(IGraph)}", Explicit = true)]
    public async Task Test2()
    {
        // Arrange
        var setups = typeof(SetupUserCapabilities).GetCapabilitiesAsNodes().First();
        var authentications = typeof(AuthenticationCapabilities).GetCapabilitiesAsNodes().First();
        var domotics = typeof(DomoticCapabilities).GetCapabilitiesAsNodes();
        var setThermostatSetpoint = domotics
            .Where(x =>
                x.Capability.OutcomeMetadata.Source.Contains(
                    nameof(DomoticCapabilities.SetThermostatSetpoint),
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .First();

        var setLightIsOn = domotics.GetByCapabilitySouce<DomoticCapabilities>(
            nameof(DomoticCapabilities.SwitchLight),
            typeof(string),
            typeof(bool)
        );

        var addThermostat = domotics.GetByCapabilitySouce<DomoticCapabilities>(
            nameof(DomoticCapabilities.AddThermostat),
            typeof(string),
            typeof(string)
        );

        var addLight = domotics.GetByCapabilitySouce<DomoticCapabilities>(
            nameof(DomoticCapabilities.AddLight),
            typeof(string),
            typeof(string)
        );

        var fullGraph = new Graph(
            [setups, authentications, setThermostatSetpoint, setLightIsOn, addLight]
        );

        var reduce = fullGraph.Reduce([setThermostatSetpoint], []);

        var fullGraphMermaid = fullGraph.ToMermaidGraph();

        var reduceMermaid = reduce.ToMermaidGraph();

        await reduceMermaid.VerifyMermaidAsync();

        // Assert
    }

    [Fact]
    public void MyTestMethod()
    {
        var host = new HostBuilder();
        host.ConfigureServices(
            (context, services) =>
            {
                services.AddHomeAutomation(context);
            }
        );
    }
}
