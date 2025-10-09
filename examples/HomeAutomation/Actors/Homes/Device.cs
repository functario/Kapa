namespace HomeAutomation.Actors.Homes;

[Actor($"A {nameof(Device)}.")]
public class Device : IDevice, IGeneratedActor
{
    public Device(Guid id, string name, string model)
    {
        Id = id;
        Name = name;
        Model = model;
    }

    [State($"The device unique identification.")]
    public Guid Id { get; set; }

    [State($"The device friendly name.")]
    public string Name { get; set; }

    [State($"The device model.")]
    public string Model { get; set; }
}
