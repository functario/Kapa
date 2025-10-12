using AwesomeAssertions;
using AwesomeAssertions.Execution;
using HomeAutomation.Actors;
using HomeAutomation.Actors.Homes;
using HomeAutomation.Capabilities;
using Kapa.Abstractions.Graphs;
using Kapa.Core.Extensions;
using Kapa.Core.Graphs;
using Kapa.Core.Validations;
using Microsoft.Testing.Platform.Capabilities;

namespace HomeAutomation.Demo;

public class Tests
{
    [Fact(DisplayName = $"{nameof(User)} is a shared instance between {nameof(ICapability)}.")]
    public async Task Test1()
    {
        // Arrange
        var user = new User();
        var timeProvider = TimeProvider.System;
        var setups = new SetupCapabilities(user);
        var authentications = new AuthenticationCapabilities(user, timeProvider);
        var domotic = new DomoticCapabilities(user);
        var tstat00Name = "Thermostat00";
        var expectedSetpoint = 17.5;

        // Act
        setups.Setup();
        var sut1 = await authentications.AuthenticateAsync("user@home.com", "1234!");
        var sut2 = await domotic.SetThermostatSetpoint(tstat00Name, expectedSetpoint);

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

    [Fact(DisplayName = $"Resolve {nameof(IGraph)}")]
    public void Test2()
    {
        // Arrange
        var user = new User();
        var timeProvider = TimeProvider.System;
        var setups = typeof(SetupCapabilities).GetCapabilitiesAsNodes().First();
        var authentications = typeof(AuthenticationCapabilities).GetCapabilitiesAsNodes().First();
        var domitics = typeof(DomoticCapabilities).GetCapabilitiesAsNodes();
        var setThermostatSetpoint = domitics
            .Where(x =>
                x.Capability.OutcomeMetadata.Source.Contains(
                    nameof(DomoticCapabilities.SetThermostatSetpoint),
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .First();

        var setLightIsOn = domitics
            .Where(x =>
                x.Capability.OutcomeMetadata.Source.Contains(
                    nameof(DomoticCapabilities.SetLightIsOn),
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .First();

        var addThermostat = domitics
            .Where(x =>
                x.Capability.OutcomeMetadata.Source.Contains(
                    nameof(DomoticCapabilities.AddThermostat),
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .First();

        var addLight = domitics
            .Where(x =>
                x.Capability.OutcomeMetadata.Source.Contains(
                    nameof(DomoticCapabilities.AddLight),
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .First();

        var graph = new Graph(
            [setups, authentications, setThermostatSetpoint, setLightIsOn, addThermostat, addLight]
        );

        var fullGraph = graph.ToMermaidGraph();

        var reduce = graph.Reduce([setLightIsOn], []).ToMermaidGraph();
        // Assert
    }
}
