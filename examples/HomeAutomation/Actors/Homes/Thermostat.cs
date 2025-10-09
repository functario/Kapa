using Kapa.Core.Actors;

namespace HomeAutomation.Actors.Homes;

[Actor($"A {nameof(Thermostat)}.")]
public class Thermostat : Device, IGeneratedActor
{
    public Thermostat(Guid id, string name, string model)
        : base(id, name, model)
    {
        Id = id;
        Name = name;
        Model = model;
    }

    [State($"The {nameof(Thermostat)} setpoint value.")]
    public double Setpoint { get; set; }

    [State($"The actual {nameof(Thermostat)} room temperature.")]
    public double Temperature { get; set; }
}
