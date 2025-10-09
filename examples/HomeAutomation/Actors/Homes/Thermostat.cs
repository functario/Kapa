namespace HomeAutomation.Actors.Homes;

public class Thermostat : Device, IGeneratedActor
{
    public Thermostat(Guid id, string name, string model)
        : base(id, name, model)
    {
        Id = id;
        Name = name;
        Model = model;
    }

    public double Setpoint { get; set; }

    public double Temperature { get; set; }
}
