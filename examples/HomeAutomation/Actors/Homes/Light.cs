using Kapa.Core.Actors;

namespace HomeAutomation.Actors.Homes;

[Actor($"A {nameof(Light)}.")]
public class Light : Device, IGeneratedActor
{
    public Light(Guid id, string name, string model)
        : base(id, name, model)
    {
        Id = id;
        Name = name;
        Model = model;
    }

    [State($"The state of the light. The true if the device is on. False otherwise.")]
    public bool IsOn { get; set; }
}
